using SharedKernel.Errors;

namespace Core.Encodings;

public static class EncodingTableErrors
{
    public static readonly Error EncodingTableEncodedContentMustBeProvide = new Error(
        $"[{nameof(EncodingTable)}]",
        "Encoding table encoded content must be provide or not be null"
    );

    public static readonly Error EncodingTableElementsMustBeProvideOrNotBeNull = new Error(
        $"[{nameof(EncodingTable)}]",
        "Encoding table elements must be provide or not be null"
    );
}