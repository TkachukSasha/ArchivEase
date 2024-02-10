using SharedKernel.Errors;

namespace Core.Encodings;

public static class EncodingTrainingErrors
{
    public static readonly Error EncodingTrainingContentMustBeProvide = Error.Validation(
       $"[{nameof(EncodingTraining)}]",
       "EncodingTraining content must be provide or not be null"
    );

    public static readonly Error EncodingTrainingLanguageMustBeProvide = Error.Validation(
      $"[{nameof(EncodingTraining)}]",
      "EncodingTraining language must be provide or not be null"
    );

    public static readonly Error EncodingTrainingAlgorithmNameMustBeProvide = Error.Validation(
       $"[{nameof(EncodingTraining)}]",
       "EncodingTraining algorithm name must be provide or not be null"
    );
}
