
using DotNet8.PosBackendApi.Features.Staff;
using DotNet8.PosBackendApi.Features.State;

namespace DotNet8.PosBackendApi;

public static class ModularService
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAppDbContextService(builder);
        services.AddJwtTokenGenerateServices();
        services.AddDataAccessServices();
        services.AddBusinessLogicServices();
        return services;
    }

    private static IServiceCollection AddAppDbContextService(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddDbContext<AppDbContext>(
            opt => { opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")); },
            ServiceLifetime.Transient,
            ServiceLifetime.Transient);

        return services;
    }

    private static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<StaffService>();
        services.AddScoped<BL_Shop>();
        services.AddScoped<BL_Staff>();
        services.AddScoped<BL_Product>();
        services.AddScoped<ShopService>();
        services.AddScoped<ResponseModel>();
        services.AddScoped<BL_ProductCategory>();
        services.AddScoped<BL_Login>();
        services.AddScoped<BL_SaleInvoice>();
        services.AddScoped<BL_Report>();
        services.AddScoped<BL_Customer>();
        services.AddScoped<BL_Township>();
        services.AddScoped<BL_State>();
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
        return services;
    }

    private static IServiceCollection AddJwtTokenGenerateServices(this IServiceCollection services)
    {
        services.AddScoped<JwtTokenGenerate>();
        return services;
    }
}