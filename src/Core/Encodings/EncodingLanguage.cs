using SharedKernel.Abstractions;
using System.Globalization;

namespace Core.Encodings;

public class EncodingLanguage : Enumeration<EncodingLanguage>
{
    public static EncodingLanguage Ukrainian = new UkrainianLanguage();
    public static EncodingLanguage English = new EnglishLanguage();

    public EncodingLanguage
    (
        Guid value,
        string name
    ) : base(value, name)
    {
    }

    private sealed class UkrainianLanguage : EncodingLanguage
    {
        private static CultureInfo UkrainianCulture = CultureInfo.GetCultureInfo("ua-UA");

        public UkrainianLanguage()
            : base(Guid.NewGuid(), UkrainianCulture.DisplayName) { }
    }

    private sealed class EnglishLanguage : EncodingLanguage
    {
        private static CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en-US");

        public EnglishLanguage()
            : base(Guid.NewGuid(), EnglishCulture.DisplayName) { }
    }
}