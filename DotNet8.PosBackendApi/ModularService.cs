using DotNet8.PosBackendApi.Features.Dashboard;
using DotNet8.PosBackendApi.Features.State;
using DotNet8.PosBackendApi.Features.Tax;

namespace DotNet8.PosBackendApi;

public static class ModularService
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddJwtTokenGenerateServices();
        services.AddDataAccessServices();
        services.AddBusinessLogicServices();
        return services;
    }

    public static IServiceCollection AddAppDbContextService(this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        },
        ServiceLifetime.Transient,
        ServiceLifetime.Transient);

        return services;
    }

    private static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<BL_Shop>();
        services.AddScoped<BL_Staff>();
        services.AddScoped<BL_Product>();
        services.AddScoped<ResponseModel>();
        services.AddScoped<BL_ProductCategory>();
        services.AddScoped<BL_Login>();
        services.AddScoped<BL_SaleInvoice>();
        services.AddScoped<BL_Report>();
        services.AddScoped<BL_Customer>();
        services.AddScoped<BL_Township>();
        services.AddScoped<BL_State>();
        services.AddScoped<BL_Tax>();
        services.AddScoped<BL_Dashboard>();
        return services;
    }

    private static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddScoped<DL_Shop>();
        services.AddScoped<DL_Staff>();
        services.AddScoped<DL_Product>();
        services.AddScoped<DL_ProductCategory>();
        services.AddScoped<DL_Login>();
        services.AddScoped<DL_SaleInvoice>();
        services.AddScoped<DL_Report>();
        services.AddScoped<DL_Customer>();
        services.AddScoped<DL_Township>();
        services.AddScoped<DL_State>();
        services.AddScoped<DL_Tax>();
        services.AddScoped<DL_Dashboard>();
        return services;
    }

    private static IServiceCollection AddJwtTokenGenerateServices(this IServiceCollection services)
    {
        services.AddScoped<JwtTokenGenerate>();
        return services;
    }

    public static WebApplicationBuilder AddJwtAuthorization(this WebApplicationBuilder builder)
    {
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
        return builder;
    }
}