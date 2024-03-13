using Core.Encodings.Builders.ShannonFano;
using Core.Encodings;

namespace Unit.Tests.Encodings.Builders;

public class ShannonFanoBuilderTests
{
    internal static string UkrMessage => "Моє ім'я Олександр";

    internal static string EngMessage => "My name is Sasha";

    [Theory]
    [InlineData("английский (Соединенные Штаты)")]
    [InlineData("ua (Ukraine)")]
    public void Should_EncodeAndDecode_MessageThatProvided(string languageName)
    {
        string message = languageName == "ua (Ukraine)" ? UkrMessage : EngMessage;

        (byte[] encodedData, EncodingTableElements encodingTableElements, Guid languageId, Guid _) = ShannonFanoEncodeBuilder
            .Init()
            .WithContent(message)
            .WithTableElements(null)
            .PrepareContent()
            .Build();

        string decodedData = ShannonFanoDecodeBuilder
            .Init()
            .WithContent(encodedData)
            .WithTableElements(encodingTableElements)
            .WithLanguage(EncodingLanguage.FromValue(languageId)!.Name)
            .PrepareContent()
            .Build();

        decodedData.Should().Be(message);
    }
}