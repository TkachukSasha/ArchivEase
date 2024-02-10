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
            ContentType = GetContentType(Path.GetExtension(fileName))
        });
    }

    public (double, string) GetFileSizeUnitsOfMeasurement(long length)
    {
        if (length == 0) return (0.0d, FileUnitsOfMeasurement.Names.B);

        string[] sizes = [
            FileUnitsOfMeasurement.Names.B,
            FileUnitsOfMeasurement.Names.KB,
            FileUnitsOfMeasurement.Names.MB,
            FileUnitsOfMeasurement.Names.GB
        ];

        int i = (int)Math.Floor(Math.Log(length) / Math.Log(1024));

        return (length / Math.Pow(1024, i), sizes[i]);
    }

    private string GetContentType(string fileExtension) =>
        fileExtension.ToLower() switch
        {
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".png" => "image/png",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",
            ".flac" => "audio/flac",
            _ => "application/octet-stream"
        };
}
