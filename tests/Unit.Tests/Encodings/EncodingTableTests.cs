using Core.Encodings;
using SharedKernel.Errors;

namespace Unit.Tests.Encodings;

public class EncodingTableTests
{
    private static byte[] byteArray => new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45 };

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Should_Failed_WhenEncodingTableElements_IsEmpty(Guid encodingLanguageId)
    {
        EncodingTableElements elements = new EncodingTableElements();

        Result<EncodingTable> encodingTable = EncodingTable.Init(byteArray, Guid.Empty, encodingLanguageId, elements);

        encodingTable.IsFailure.Should().BeTrue();
        encodingTable.Errors.Should().Contain(EncodingTableErrors.EncodingTableElementsMustBeProvideOrNotBeNull);
    }

    [Fact]
    public void Should_Failed_WhenEncodingTableElements_AreNull()
    {
        Result<EncodingTable> encodingTable = EncodingTable.Init(byteArray, Guid.Empty, Guid.Empty, null);

        encodingTable.IsFailure.Should().BeTrue();
        encodingTable.Errors.Should().Contain(EncodingTableErrors.EncodingTableElementsMustBeProvideOrNotBeNull);
    }
}