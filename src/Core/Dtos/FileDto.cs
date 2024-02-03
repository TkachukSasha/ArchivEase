namespace Core.Dtos;

public record FileDto
(
    string FileName,
    byte DefaultSize,
    byte EncodedSize,
    string FilePath,
    string FileUnitsOfMeasurement
);

public record FileEntryDto
(
    string FileName,
    string ContentType,
    long FileLength,
    byte DefaultSize,
    Stream Stream
);