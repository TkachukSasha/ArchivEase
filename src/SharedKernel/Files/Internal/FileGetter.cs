using SharedKernel.Errors;

namespace SharedKernel.Files.Internal;

internal sealed class FileGetter : IFileGetter
{
    private readonly string _storagePath;

    public FileGetter(FileOptions options) => _storagePath = options.Path;

    public async Task<Result> GetFileBytesAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure(FileErrors.FileNameMustBeProvide);

        var filePath = Path.Combine(_storagePath, fileName);

        if (!File.Exists(filePath))
            return Result.Failure(FileErrors.FileNotFound(filePath));

        var fileBytes = await File.ReadAllBytesAsync(filePath);

        return Result.Success(new FileResponse
        {
            Bytes = fileBytes,
            ContentType = GetContentType(Path.GetExtension(fileName))
        });
    }

    public string GetFileUnitsOfMeasurement(long length) =>
        length switch
        {
            var l when l < FileUnitsOfMeasurement.Size.KB => FileUnitsOfMeasurement.Names.B,
            var l when l < FileUnitsOfMeasurement.Size.MB => FileUnitsOfMeasurement.Names.KB,
            var l when l < FileUnitsOfMeasurement.Size.GB => FileUnitsOfMeasurement.Names.MB,
            _ => FileUnitsOfMeasurement.Names.GB
        };

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
