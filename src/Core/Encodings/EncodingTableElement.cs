using SharedKernel.Errors;
using System.Text.Json.Serialization;

namespace Core.Encodings;

public sealed class EncodingTableElement
{
    private EncodingTableElement() { }

    [JsonConstructor]
    private EncodingTableElement(
        char symbol,
        string code
    )
    {
        Symbol = symbol;
        Code = code;
    }

    public char Symbol { get; }

    public string Code { get; }

    public static Result<EncodingTableElement> Init(
        char symbol,
        string code
    ) =>
        Result.Ensure(
            code,
            (c => !string.IsNullOrWhiteSpace(c), EncodingTableElementErrors.EncodingTableElementCodeMustBeProvide))
            .Map(_ => new EncodingTableElement(symbol, code)
        );
}

public sealed class EncodingTableElements : List<EncodingTableElement>
{
}