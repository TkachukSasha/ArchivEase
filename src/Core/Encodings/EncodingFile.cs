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
        string fileUnitsOfMeasurement,
        string contentType,
        byte encodedSize,
        byte defaultSize
    ) : base(id)
    {
        EncodingTableId = encodingTableId;
        FilePath = filePath;
        FileName = fileName;
        FileUnitsOfMeasurement = fileUnitsOfMeasurement;
        ContentType = contentType;
        EncodedSize = encodedSize;
        DefaultSize = defaultSize;
    }

    public string FilePath { get; }

    public string FileName { get; }

    public string FileUnitsOfMeasurement { get; }

    public string ContentType { get; }

    public byte EncodedSize { get; }

    public byte DefaultSize { get; }

    public Guid EncodingTableId { get; }

    public EncodingTable? EncodingTable { get; }

    public static Result<EncodingFile> Init(
        Guid encodingTableId,
        string filePath,
        string fileName,
        string fileUnitsOfMeasurement,
        string contentType,
        byte encodedSize,
        byte defaultSize
    ) =>
        Result.Ensure(
            (encodingTableId, filePath, contentType, fileName),
            (_ => encodingTableId != Guid.Empty, EncodingFileErrors.EncodingFileTableIdMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(filePath), EncodingFileErrors.EncodingFilePathMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(fileName), EncodingFileErrors.EncodingFileNameMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(fileUnitsOfMeasurement), EncodingFileErrors.EncodingFileUnitsOfMeasurementMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(contentType), EncodingFileErrors.EncodingFileContentTypeMustBeProvide)
        )
        .Map(_ => new EncodingFile(new EncodingFileId(), encodingTableId, filePath, fileName, fileUnitsOfMeasurement, contentType, encodedSize, defaultSize));
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