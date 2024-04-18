namespace DotNet8.PosBackendApi;

public static class ModularService
{
    public static IServiceCollection AddService(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAppDbContextService(builder);
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
        return services;
    }

    private static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddScoped<DL_Shop>();
        services.AddScoped<DL_Staff>();
        services.AddScoped<DL_Product>();
        services.AddScoped<DL_ProductCategory>();
        return services;
    }
}