var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7136", "http://localhost:5065") // or AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});


string projectDirectory = Environment.CurrentDirectory;
var builderJwtSetting = new ConfigurationBuilder();
builderJwtSetting.SetBasePath(projectDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfiguration configJwtSetting = builderJwtSetting.Build();
builder.Services.Configure<JwtModel>(configJwtSetting.GetSection("Jwt"));

#region Register Services

builder.Services.AddScoped(n => new DapperService(builder.Configuration.GetConnectionString("DbConnection")!));
builder.Services.AddServices(builder);

#endregion

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNet8.PosBackendApi", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
           new List<string> ()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();