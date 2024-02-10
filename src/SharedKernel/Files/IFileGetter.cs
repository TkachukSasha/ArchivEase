using SharedKernel.Errors;

namespace SharedKernel.Files;

public interface IFileGetter
{
    Task<Result<FileResponse>> GetFileBytesAsync(string fileName);

    (double, string) GetFileSizeUnitsOfMeasurement(long length);
}
