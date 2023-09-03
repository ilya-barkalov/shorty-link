using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SL.Application.Common.Interfaces;
using SL.Infrastructure.Persistence;

namespace SL.Application.UnitTests;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryContext(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationDbContext, ApplicationDbContext>(serviceProvider =>
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseApplicationServiceProvider(serviceProvider)
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();

            return context;
        });

        return services;
    }
}