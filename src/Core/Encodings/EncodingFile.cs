using SharedKernel.Abstractions;
using SharedKernel.Errors;

namespace Core.Encodings;

public sealed class EncodingFile : Entity<EncodingFileId>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private EncodingFile()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    private EncodingFile
    (
        Guid id,
        Guid encodingTableId,
        string filePath,
        string fileName,
        string encodedFileUnitsOfMeasurement,
        string defaultFileUnitsOfMeasurement,
        string contentType,
        double encodedSize,
        double defaultSize,
        Guid userId
    ) : base(id)
    {
        EncodingTableId = encodingTableId;
        FilePath = filePath;
        FileName = fileName;
        EncodedFileUnitsOfMeasurement = encodedFileUnitsOfMeasurement;
        DefaultFileUnitsOfMeasurement = defaultFileUnitsOfMeasurement;
        ContentType = contentType;
        EncodedSize = encodedSize;
        DefaultSize = defaultSize;
        UserId = userId;
    }

    public string FilePath { get; }

    public string FileName { get; }

    public string EncodedFileUnitsOfMeasurement { get; }

    public string DefaultFileUnitsOfMeasurement { get; }

    public string ContentType { get; }

    public double EncodedSize { get; }

    public double DefaultSize { get; }

    public Guid EncodingTableId { get; }

    public Guid UserId { get; }

    public static Result<EncodingFile> Init(
        Guid encodingTableId,
        string filePath,
        string fileName,
        string encodedFileUnitsOfMeasurement,
        string defaultFileUnitsOfMeasurement,
        string contentType,
        double encodedSize,
        double defaultSize,
        Guid userId
    ) =>
        Result.Ensure(
            (encodingTableId, filePath, contentType, fileName),
            (_ => encodingTableId != Guid.Empty, EncodingFileErrors.EncodingFileTableIdMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(filePath), EncodingFileErrors.EncodingFilePathMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(fileName), EncodingFileErrors.EncodingFileNameMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(encodedFileUnitsOfMeasurement), EncodingFileErrors.EncodingEncodedFileUnitsOfMeasurementMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(defaultFileUnitsOfMeasurement), EncodingFileErrors.EncodingDefaultFileUnitsOfMeasurementMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(contentType), EncodingFileErrors.EncodingFileContentTypeMustBeProvide)
        )
        .Map(_ => new EncodingFile(new EncodingFileId(), encodingTableId, filePath, fileName, encodedFileUnitsOfMeasurement, defaultFileUnitsOfMeasurement, contentType, encodedSize, defaultSize, userId));
}

public sealed class EncodingFileId : TypeId
{
    public EncodingFileId()
        : this(Guid.NewGuid()) { }

    public EncodingFileId(Guid value) : base(value) { }

    public static implicit operator EncodingFileId(Guid id) => new(id);

    public static implicit operator Guid(EncodingFileId id) => id.Value;
    public override string ToString() => Value.ToString();
}