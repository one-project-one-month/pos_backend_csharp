var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

#region Register Services

builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<DL_Shop>();
builder.Services.AddScoped<BL_Shop>();
builder.Services.AddScoped<DL_Staff>();
builder.Services.AddScoped<BL_Staff>();
builder.Services.AddScoped<DL_Product>();
builder.Services.AddScoped<BL_Product>();
builder.Services.AddScoped<ShopService>();
builder.Services.AddScoped<ResponseModel>();
builder.Services.AddScoped<DL_ProductCategory>();
builder.Services.AddScoped<BL_ProductCategory>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();