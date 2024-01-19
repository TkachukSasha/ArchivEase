using SharedKernel.Errors;

namespace Core.Encodings;

public static class EncodingTableElementErrors
{
    public static readonly Error EncodingTableElementCodeMustBeProvide = Error.Validation(
        $"[{nameof(EncodingTableElements)}]",
        "Encoding table element code must be provide"
    );
}