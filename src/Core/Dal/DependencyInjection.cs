using Core.Dal.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Dal;

namespace Core.Dal;

internal static class DependencyInjection
{
    internal static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ArchivEaseContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlActions =>
            {
                sqlActions.CommandTimeout(30);
            });

            options.EnableSensitiveDataLogging(true);
        });

        services.AddDal<ArchivEaseContext>();

        services
            .AddInitializer<UsersInitializer>()
            .AddInitializer<EncodingsInitializer>();

        return services;
    }

    private static IServiceCollection AddInitializer<T>(this IServiceCollection services) where T : class, IDataInitializer
       => services.AddTransient<IDataInitializer, T>();
}