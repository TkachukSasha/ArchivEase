using SharedKernel.Errors;

namespace Core.Encodings;

public static class EncodingFileErrors
{
    public static readonly Error EncodingFileTableIdMustBeProvide = Error.Validation(
        $"[{nameof(EncodingFile)}]",
        "EncodingFile tableId must be provide or not be null"
    );

    public static readonly Error EncodingFilePathMustBeProvide = Error.Validation(
        $"[{nameof(EncodingFile)}]",
        "EncodingFile path must be provide or not be null"
    );

    public static readonly Error EncodingFileNameMustBeProvide = Error.Validation(
      $"[{nameof(EncodingFile)}]",
      "EncodingFile name must be provide or not be null"
    );

    public static readonly Error EncodingFileUnitsOfMeasurementMustBeProvide = Error.Validation(
      $"[{nameof(EncodingFile)}]",
      "EncodingFile units of measurement must be provide or not be null"
    );

    public static readonly Error EncodingFileContentTypeMustBeProvide = Error.Validation(
       $"[{nameof(EncodingFile)}]",
       "EncodingFile content-type must be provide or not be null"
    );
}