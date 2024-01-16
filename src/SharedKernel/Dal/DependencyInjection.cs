using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Dal.Internal;

namespace SharedKernel.Dal;

public static class DependencyInjection
{
    public static IServiceCollection AddDal<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddHostedService<DatabaseInitializer<TContext>>();
        services.AddHostedService<DataInitializer>();

        services.AddScoped<IUnitOfWork, PostgresUnitOfWork<TContext>>();

        return services;
    }
}