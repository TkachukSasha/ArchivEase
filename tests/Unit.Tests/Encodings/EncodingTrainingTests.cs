using Core.Encodings;
using SharedKernel.Errors;
using System.Globalization;

namespace Unit.Tests.Encodings;

public class EncodingTrainingTests
{
    private static readonly string EnglishLanguage = EncodingLanguage.English.Name;
    private static readonly string UkrainianLanguage = EncodingLanguage.Ukrainian.Name;
    private static readonly string VariableLengthEncoding = EncodingAlgorithm.VariableLengthCodeAlgorithm.Name;

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Failed_WhenEncodingTrainingContent_IsNullOrWhiteSpace(string content)
    {
        Result<EncodingTraining> encodingTraining = EncodingTraining.Init(content, "", "test");

        encodingTraining.IsFailure.Should().BeTrue();
        encodingTraining.Errors.Should().Contain(EncodingTrainingErrors.EncodingTrainingContentMustBeProvide);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Failed_WhenEncodingTrainingAlgorithm_IsNullOrWhiteSpace(string algorithm)
    {
        Result<EncodingTraining> encodingTraining = EncodingTraining.Init("test", "", algorithm);

        encodingTraining.IsFailure.Should().BeTrue();
        encodingTraining.Errors.Should().Contain(EncodingTrainingErrors.EncodingTrainingAlgorithmNameMustBeProvide);
    }

    [Fact]
    public void Should_Successfully_GenerateTrainingDataSet_VariableLengthEncoding()
    {
        IEnumerable<EncodingTraining> variableLengthEncodingEnglishDataSet = EncodingTraining
            .GenerateTextFilesContentVariableLengthEncoding(EnglishLanguage, VariableLengthEncoding, 1000);

        IEnumerable<EncodingTraining> variableLengthEncodingUkrainianDataSet = EncodingTraining
           .GenerateTextFilesContentVariableLengthEncoding(UkrainianLanguage, VariableLengthEncoding, 1000);

        variableLengthEncodingEnglishDataSet.Should().NotBeEmpty();
        variableLengthEncodingUkrainianDataSet.Should().NotBeEmpty();
    }
}
