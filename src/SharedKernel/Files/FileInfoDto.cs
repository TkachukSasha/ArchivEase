namespace SharedKernel.Files;

public sealed class FileInfoDto
{
    public string FilePath { get; set; }
    public string EncodedUnitsOfMeasurement { get; set; }

    public double FileLength { get; set; }
}
