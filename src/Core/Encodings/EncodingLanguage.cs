using SharedKernel.Abstractions;

namespace Core.Encodings;

public class EncodingLanguage : Enumeration<EncodingLanguage>
{
    public static EncodingLanguage Ukrainian = new UkrainianLanguage();
    public static EncodingLanguage English = new EnglishLanguage();

    public EncodingLanguage(
        Guid value,
        string name
    ) : base(value, name)
    {
    }

    private sealed class UkrainianLanguage : EncodingLanguage
    {
        public UkrainianLanguage()
            : base(Guid.NewGuid(), "ua-UA") { }
    }

    private sealed class EnglishLanguage : EncodingLanguage
    {
        public EnglishLanguage()
            : base(Guid.NewGuid(), "en-US") { }
    }
}