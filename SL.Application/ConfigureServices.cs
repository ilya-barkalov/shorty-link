using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SL.Application.Services;

namespace SL.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly());
        });

        services.AddTransient<RandomStringService>();
        
        return services;
    }
}