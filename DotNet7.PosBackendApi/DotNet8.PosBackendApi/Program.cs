using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// var secret = Configuration["Jwt:Secret"];
// var key = Encoding.ASCII.GetBytes(secret);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//string projectDirectory = Environment.CurrentDirectory;
//var config = builder
//    .Configuration
//    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//    .Build();
//builder.Services.Configure<TokenModel>(config.GetSection("Jwt"));

string projectDirectory = Environment.CurrentDirectory;
var builderJwtSetting = new ConfigurationBuilder();
builderJwtSetting.SetBasePath(projectDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfiguration configJwtSetting = builderJwtSetting.Build();
builder.Services.Configure<JwtModel>(configJwtSetting.GetSection("Jwt"));

#region Register Services

builder.Services.AddService(builder);

#endregion

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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
            new string[] { }
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]))
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();