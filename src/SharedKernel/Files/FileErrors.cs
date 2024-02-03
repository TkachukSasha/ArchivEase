using SharedKernel.Errors;

namespace SharedKernel.Files;

public static class FileErrors
{
    public static readonly Error FileNameMustBeProvide = Error.Validation(
         "[File]",
         "File name must be provide"
    );

    public static Error FileNotFound(string filePath) =>
        Error.NotFound(
             "[File]",
             $"File by path -> {filePath} not found"
        );
}
