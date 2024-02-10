using SharedKernel.Errors;

namespace SharedKernel.Files;

public interface IFileSetter
{
    Task<Result<FileInfoDto>> SetFileAsync(Stream stream, string fileName);
}
