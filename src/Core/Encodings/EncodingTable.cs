using SharedKernel.Abstractions;
using SharedKernel.Errors;
using SharedKernel.Types;

namespace Core.Encodings;

public sealed class EncodingTable : Entity<EncodingTableId>
{
    private EncodingTable(
        Guid id,
        byte[]? encodedContentBytes,
        Guid encodingAlgorithmId,
        Guid? encodingLanguageId,
        EncodingTableElements encodingTableElements
    ) : base(id)
    {
        EncodedContentBytes = encodedContentBytes;
        EncodingAlgorithmId = encodingAlgorithmId;
        EncodingLanguageId = encodingLanguageId;
        EncodingTableElements = encodingTableElements;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private EncodingTable()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public byte[]? EncodedContentBytes { get; set; }

    public Guid EncodingAlgorithmId { get; }

    public Guid? EncodingLanguageId { get; }

    public EncodingTableElements EncodingTableElements { get; }

    public static Result<EncodingTable> Init(
        byte[]? encodedContentBytes,
        Guid encodingAlgorithmId,
        Guid? encodingLanguageId,
        EncodingTableElements encodingTableElements
    ) =>
        Result.Ensure(
            (encodedContentBytes, encodingTableElements),
            (_ => !encodedContentBytes.ArrayOfBytesIsNullOrEmpty(), EncodingTableErrors.EncodingTableEncodedContentMustBeProvide),
            (_ => encodingTableElements is not null && encodingTableElements.Any(), EncodingTableErrors.EncodingTableElementsMustBeProvideOrNotBeNull)
        )
        .Map(_ => new EncodingTable(new EncodingTableId(), encodedContentBytes, encodingAlgorithmId, encodingLanguageId, encodingTableElements));
}

public sealed class EncodingTableId : TypeId
{
    public EncodingTableId()
        : this(Guid.NewGuid()) { }

    public EncodingTableId(Guid value) : base(value) { }

    public static implicit operator EncodingTableId(Guid id) => new(id);

    public static implicit operator Guid(EncodingTableId id) => id.Value;
    public override string ToString() => Value.ToString();
}