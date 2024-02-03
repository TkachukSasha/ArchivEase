using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Files.Internal;

namespace SharedKernel.Files;

public static class DependencyInjection
{
    public static FileOptions ConfigureFileOptions
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var options = configuration.BindOptions<FileOptions>("Storage");

        services.AddSingleton( options );

        return options;
    }

    public static IServiceCollection AddFiles(this IServiceCollection services)
        => services
                .AddSingleton<IFileGetter, FileGetter>()
                .AddSingleton<IFileSetter, FileSetter>();
}
