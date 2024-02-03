using SharedKernel.Errors;

namespace SharedKernel.Files;

public interface IFileSetter
{
    Task<Result<string>> SetFileAsync(Stream stream, string fileName);
}
