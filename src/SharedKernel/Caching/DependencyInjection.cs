using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Caching.Internal;

namespace SharedKernel.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddInMemoryCaching(this IServiceCollection services)
         => services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
}
