using SharedKernel;

namespace Api.Common.Cors;

internal static class DependencyInjection
{
    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.BindOptions<CorsOptions>("Cors");
        services.AddSingleton(options);

        return services.AddCors(cors =>
        {
            var allowedHeaders = options.AllowedHeaders;
            var allowedMethods = options.AllowedMethods;
            var allowedOrigins = options.AllowedOrigins;

            cors.AddPolicy("Cors", corsPolicy =>
            {
                var origins = allowedOrigins?.ToArray() ?? Array.Empty<String>();

                if (options.AllowCredentials && origins.FirstOrDefault() != "*")
                    corsPolicy.AllowCredentials();
                else
                    corsPolicy.DisallowCredentials();

                corsPolicy
                    .WithHeaders(allowedHeaders?.ToArray() ?? Array.Empty<string>())
                    .WithMethods(allowedMethods?.ToArray() ?? Array.Empty<string>())
                    .WithOrigins(origins.ToArray());
            });
        });
    }

    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        => app.UseCors("Cors");
}
