using SharedKernel.Errors;

namespace SharedKernel.Files;

public interface IFileGetter
{
    Task<Result> GetFileBytesAsync(string fileName);

    string GetFileUnitsOfMeasurement(long length);
}
