using Core.Dal.Initializers.Internal;
using Core.Encodings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Dal;

namespace Core.Dal.Initializers;

internal sealed class EncodingsInitializer : BaseInitializer<ArchivEaseContext, EncodingsInitializer>, IDataInitializer
{
    private static readonly string EnglishLanguage = EncodingLanguage.English.Name;
    private static readonly string UkrainianLanguage = EncodingLanguage.Ukrainian.Name;
    private static readonly string VariableLengthEncoding = EncodingAlgorithm.VariableLengthCodeAlgorithm.Name;

    public EncodingsInitializer(ArchivEaseContext context, ILogger<EncodingsInitializer> logger)
        : base(context, logger)
    {
    }

    public async Task InitAsync()
    {
        if (!await _context.EncodingTrainings.AnyAsync()) await InitEncodingTrainingsAsync();

        if (!await _context.EncodingLanguages.AnyAsync()) await InitEncodingLanguagesAsync();

        if (!await _context.EncodingAlgorithms.AnyAsync()) await InitEncodingAlgorihmsAsync();

        await _context.SaveChangesAsync();
    }

    private async Task InitEncodingTrainingsAsync()
    {
        IEnumerable<EncodingTraining> variableLengthEncodingEnglishDataSet = EncodingTraining
            .GenerateTextFilesContentVariableLengthEncoding(EnglishLanguage, VariableLengthEncoding, 1000);

        IEnumerable<EncodingTraining> variableLengthEncodingUkrainianDataSet = EncodingTraining
           .GenerateTextFilesContentVariableLengthEncoding(UkrainianLanguage, VariableLengthEncoding, 1000);

        IEnumerable<EncodingTraining> variableLengthEncodingDataSetItems = variableLengthEncodingEnglishDataSet.Union(variableLengthEncodingUkrainianDataSet);

        await _context.EncodingTrainings.AddRangeAsync(variableLengthEncodingDataSetItems);

        _logger.Log(LogLevel.Information, "encoding_training initialized succesessfully!");
    }

    private async Task InitEncodingLanguagesAsync()
    {
        await _context.EncodingLanguages.AddRangeAsync(EncodingLanguage.GetValues());

        _logger.Log(LogLevel.Information, "encoding_languages initialized succesessfully!");
    }

    private async Task InitEncodingAlgorihmsAsync()
    {
        await _context.EncodingAlgorithms.AddRangeAsync(EncodingAlgorithm.GetValues());

        _logger.Log(LogLevel.Information, "encoding_algorithms initialized succesessfully!");
    }
}