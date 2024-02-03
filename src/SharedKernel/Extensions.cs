using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace SharedKernel;

public static class Extensions
{
    public static IFileProvider SafeCreateStaticFileProvider(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var fileProvider = new PhysicalFileProvider(path);

        return fileProvider;
    }

    public static T BindOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        => BindOptions<T>(configuration.GetSection(sectionName));

    public static T BindOptions<T>(this IConfigurationSection section) where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }
}