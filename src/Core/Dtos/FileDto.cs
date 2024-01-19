namespace Core.Dtos;

public record FileDto
(
    string FileName,
    byte DefaultSize,
    byte EncodingSize,
    string FilePath,
    string FileUnitsOfMeasurement
);