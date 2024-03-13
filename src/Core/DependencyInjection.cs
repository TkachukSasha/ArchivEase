using Core.Dal;
using Core.Encodings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Authentication;
using SharedKernel.Caching;
using SharedKernel.Commands;
using SharedKernel.Dispatchers;
using SharedKernel.Files;
using SharedKernel.Queries;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        services.AddCommands();

        services.AddTransactionalCommandDecorator();

        services.AddQueries();

        services.AddPagedQueryDecorator();

        services.AddDispatchers();

        services.AddInMemoryCaching();

        services.AddSingleton<EncodingAnalyzer>();

        services.AddDataLayer(configuration.GetConnectionString("Default")!);

        services.AddAuth(configuration);

        services.AddFiles();

        return services;
    }
}
