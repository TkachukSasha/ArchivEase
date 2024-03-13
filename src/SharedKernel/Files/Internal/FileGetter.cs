using SharedKernel.Errors;

namespace SharedKernel.Files.Internal;

internal sealed class FileGetter : IFileGetter
{
    private readonly string _storagePath;

    public FileGetter(FileOptions options) => _storagePath = options.Path;

    public async Task<Result<FileResponse>> GetFileBytesAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure<FileResponse>(FileErrors.FileNameMustBeProvide);

        var filePath = Path.Combine(_storagePath, fileName);

        if (!File.Exists(filePath))
            return Result.Failure<FileResponse>(FileErrors.FileNotFound(filePath));

        var fileBytes = await File.ReadAllBytesAsync(filePath);

        return Result.Success(new FileResponse
        {
            Bytes = fileBytes,
            ContentType = FileExtensions.GetContentType(Path.GetExtension(fileName))
        });
    }
}
