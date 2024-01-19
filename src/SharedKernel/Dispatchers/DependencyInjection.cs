using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Dispatchers.Internal;

namespace SharedKernel.Dispatchers;

public static class DependencyInjection
{
    public static IServiceCollection AddDispatchers(this IServiceCollection services)
        => services.AddSingleton<IDispatcher, Dispatcher>();
}