using Core.Encodings.Builders.ShannonFano;
using Core.Encodings;

namespace Unit.Tests.Encodings.Builders;

public class ShannonFanoBuilderTests
{
    internal static string UkrMessage => "Моє ім'я Олександр";

    internal static string EngMessage => "My name is Sasha";

    [Theory]
    [InlineData("ua-UA")]
    [InlineData("en-US")]
    public void Should_EncodeAndDecode_MessageThatProvided(string languageName)
    {
        string message = languageName == "ua-UA" ? UkrMessage : EngMessage;

        (string encodedData, EncodingTableElements encodingTableElements) = ShannonFanoEncodeBuilder
            .Init()
            .WithContent(message)
            .WithTableElements(null)
            .PrepareContent()
            .Build();

        string decodedData = ShannonFanoDecodeBuilder
            .Init()
            .WithContent(encodedData)
            .WithTableElements(encodingTableElements)
            .PrepareContent()
            .Build();

        decodedData.Should().Be(message);
    }
}