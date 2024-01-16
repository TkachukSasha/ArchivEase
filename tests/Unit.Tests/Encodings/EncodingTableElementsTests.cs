using Core.Encodings;
using SharedKernel.Errors;

namespace Unit.Tests.Encodings;

public class EncodingTableElementsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Failed_WhenEncodingTableElementCode_IsNullOrWhiteSpace(string code)
    {
        Result<EncodingTableElement> encodingTableElement = EncodingTableElement.Init(' ', code);

        encodingTableElement.IsFailure.Should().BeTrue();
        encodingTableElement.Errors.Should().Contain(EncodingTableElementErrors.EncodingTableElementCodeMustBeProvide);
    }
}