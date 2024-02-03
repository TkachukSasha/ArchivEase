using SharedKernel.Errors;

namespace SharedKernel.Files.Internal;

internal sealed class FileSetter : IFileSetter
{
    private readonly string _storagePath;

    public FileSetter(FileOptions options) => _storagePath = options.Path;

    public async Task<Result<string>> SetFileAsync(Stream stream, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure<string>(FileErrors.FileNameMustBeProvide);

        var filePath = Path.Combine(_storagePath, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }

        return Result.Success(filePath);
    }
}
