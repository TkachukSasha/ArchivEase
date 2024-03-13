using SharedKernel.Errors;

namespace SharedKernel.Files;

public interface IFileGetter
{
    Task<Result<FileResponse>> GetFileBytesAsync(string fileName);
}
