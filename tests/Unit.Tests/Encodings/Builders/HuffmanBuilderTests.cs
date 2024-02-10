using Core.Encodings.Builders.Huffman;
using Core.Encodings;

namespace Unit.Tests.Encodings.Builders;

public class HuffmanBuilderTests
{
    internal static string UkrMessage => "Моє ім'я Олександр";

    internal static string EngMessage => "My name is Sasha";

    [Theory]
    [InlineData("английский (Соединенные Штаты)")]
    [InlineData("ua (Ukraine)")]
    public void Should_EncodeAndDecode_MessageThatProvided(string languageName)
    {
        string message = languageName == "ua (Ukraine)" ? UkrMessage : EngMessage;

        (byte[] encodedData, EncodingTableElements encodingTableElements) = HuffmanEncodeBuilder
            .Init()
            .WithContent(message)
            .WithTableElements(null)
            .PrepareContent()
            .Build();

        string decodedData = HuffmanDecodeBuilder
            .Init()
            .WithContent(encodedData)
            .WithTableElements(encodingTableElements)
            .PrepareContent()
            .Build();

        decodedData.Should().Be(message);
    }
}