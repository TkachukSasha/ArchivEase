namespace Core.Dtos;

public record FileDto
(
    string FileName,
    double DefaultSize,
    double EncodedSize,
    string FilePath,
    string EncodedFileUnitsOfMeasurement,
    string DefaultFileUnitsOfMeasurement
);

public record FileEntryDto
(
     string FileName,
     string ContentType,
     long Length,
     Stream Stream
);