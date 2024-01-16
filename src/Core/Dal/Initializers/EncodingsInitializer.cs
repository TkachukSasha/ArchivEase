using Core.Dal.Initializers.Internal;
using Core.Encodings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Dal;

namespace Core.Dal.Initializers;

internal sealed class EncodingsInitializer : BaseInitializer<ArchivEaseContext, EncodingsInitializer>, IDataInitializer
{
    internal EncodingsInitializer(ArchivEaseContext context, ILogger<EncodingsInitializer> logger)
        : base(context, logger)
    {
    }

    public async Task InitAsync()
    {
        if (!await _context.EncodingLanguages.AnyAsync()) await InitEncodingLanguagesAsync();

        if (!await _context.EncodingAlgorithms.AnyAsync()) await InitEncodingAlgorihmsAsync();

        await _context.SaveChangesAsync();
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