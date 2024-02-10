using Core.Encodings;
using SharedKernel.Errors;

namespace Unit.Tests.Encodings;

public class EncodingFileTests
{
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Should_Failed_WhenEncodingFile_TableId_IsEmpty(Guid encodingLanguageId)
    {
        Result<EncodingFile> encodingFile = EncodingFile.Init(encodingLanguageId, "test", "test", "test", "test", ".txt", 2, 5);

        encodingFile.IsFailure.Should().BeTrue();
        encodingFile.Errors.Should().Contain(EncodingFileErrors.EncodingFileTableIdMustBeProvide);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenEncodingFile_FilePath_IsNullOrWhiteSpace(string filePath)
    {
        Result<EncodingFile> encodingFile = EncodingFile.Init(Guid.NewGuid(), filePath, "test", "test", "test", ".txt", 2, 5);

        encodingFile.IsFailure.Should().BeTrue();
        encodingFile.Errors.Should().Contain(EncodingFileErrors.EncodingFilePathMustBeProvide);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenEncodingFile_ContentType_IsNullOrWhiteSpace(string contentType)
    {
        Result<EncodingFile> encodingFile = EncodingFile.Init(Guid.NewGuid(), "test", contentType, "test", "test", ".txt", 2, 5);

        encodingFile.IsFailure.Should().BeTrue();
        encodingFile.Errors.Should().Contain(EncodingFileErrors.EncodingFileContentTypeMustBeProvide);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenEncodingFile_FileName_IsNullOrWhiteSpace(string fileName)
    {
        Result<EncodingFile> encodingFile = EncodingFile.Init(Guid.NewGuid(), "test", "test", "test", fileName, ".txt", 2, 5);

        encodingFile.IsFailure.Should().BeTrue();
        encodingFile.Errors.Should().Contain(EncodingFileErrors.EncodingFileNameMustBeProvide);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenEncodingFile_EncodedFileUnitsOfMeasurements_IsNullOrWhiteSpace(string encodedFileUnitsOfMeasurements)
    {
        Result<EncodingFile> encodingFile = EncodingFile.Init(Guid.NewGuid(), "test", "test", encodedFileUnitsOfMeasurements, "test", ".txt", 2, 5);

        encodingFile.IsFailure.Should().BeTrue();
        encodingFile.Errors.Should().Contain(EncodingFileErrors.EncodingEncodedFileUnitsOfMeasurementMustBeProvide);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenEncodingFile_DefaultFileUnitsOfMeasurements_IsNullOrWhiteSpace(string defaultFileUnitsOfMeasurements)
    {
        Result<EncodingFile> encodingFile = EncodingFile.Init(Guid.NewGuid(), "test", "test", "test", defaultFileUnitsOfMeasurements, ".txt", 2, 5);

        encodingFile.IsFailure.Should().BeTrue();
        encodingFile.Errors.Should().Contain(EncodingFileErrors.EncodingDefaultFileUnitsOfMeasurementMustBeProvide);
    }
}