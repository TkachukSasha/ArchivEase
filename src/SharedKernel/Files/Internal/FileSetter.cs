using SharedKernel.Errors;
using System.IO;

namespace SharedKernel.Files.Internal;

internal sealed class FileSetter : IFileSetter
{
    private readonly string _storagePath;

    public FileSetter(FileOptions options) => _storagePath = options.Path;

    public async Task<Result<FileInfoDto>> SetFileAsync(Stream stream, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure<FileInfoDto>(FileErrors.FileNameMustBeProvide);

        var filePath = Path.Combine(_storagePath, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }

        var fileLength = new FileInfo(filePath).Length;

        (double encodedFileLength, string encodedUnitsOfMeasurement) = FileExtensions.GetFileSizeUnitsOfMeasurement(fileLength);

        return Result.Success(new FileInfoDto
        {
            FileLength = encodedFileLength,
            FilePath = filePath,
            EncodedUnitsOfMeasurement = encodedUnitsOfMeasurement
        });
    }
}
