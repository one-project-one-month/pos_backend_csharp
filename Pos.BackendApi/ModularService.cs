using Pos.BackendApi.Features.Authentication.Register;
using Pos.BackendApi.Features.Dashboard;
using Pos.BackendApi.Features.Generate;
using Pos.BackendApi.Features.State;
using Pos.BackendApi.Features.Tax;
using Pos.BackendApi.Features.SaleDraft;

namespace Pos.BackendApi;

public static class ModularService
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddJwtTokenGenerateServices();
        services.AddFeatureServices();
        return services;
    }

    public static IServiceCollection AddAppDbContextService(this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        },
        ServiceLifetime.Scoped,
        ServiceLifetime.Scoped);

        return services;
    }

    private static IServiceCollection AddFeatureServices(this IServiceCollection services)
    {
        services.AddScoped<ShopService>();
        services.AddScoped<StaffService>();
        services.AddScoped<ProductService>();
        services.AddScoped<ResponseModel>();
        services.AddScoped<ProductCategoryService>();
        services.AddScoped<LoginService>();
        services.AddScoped<RegisterService>();
        services.AddScoped<GenerateService>();
        services.AddScoped<SaleInvoiceService>();
        services.AddScoped<ReportService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<TownshipService>();
        services.AddScoped<StateService>();
        services.AddScoped<TaxService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<SaleDraftService>();
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
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Pos.BackendApi", Version = "v1" });
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
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.FromSeconds(30),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        return builder;
    }
}
