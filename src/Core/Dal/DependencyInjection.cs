using Core.Dal.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Dal;

namespace Core.Dal;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ArchivEaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddDal<ArchivEaseContext>();

        services
            .AddInitializer<UsersInitializer>()
            .AddInitializer<EncodingsInitializer>();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }

    private static IServiceCollection AddInitializer<T>(this IServiceCollection services) where T : class, IDataInitializer
       => services.AddTransient<IDataInitializer, T>();
}