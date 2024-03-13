using SharedKernel.Commands;
using SharedKernel.Errors;
using Core.Dtos;
using Core.Dal;
using Core.Encodings;
using System.Text;
using Core.Commands.Base;

namespace Core.Commands;

public sealed record DecodeCommand
(
    List<FileEntryDto> FileEntries
) : ICommand<Result<byte[]>>;

public sealed record DecodeByFileNameCommand
(
    string FileName
) : ICommand<Result<byte[]>>;

internal sealed class DecodeCommandHandler : BaseEncoding, ICommandHandler<DecodeCommand, Result<byte[]>>
{
    public DecodeCommandHandler
    (
        ArchivEaseContext context,
        EncodingAnalyzer analyzer
    ) : base (context, analyzer)    
    {
    }

    public async Task<Result<byte[]>> HandleAsync(DecodeCommand command, CancellationToken cancellationToken = default)
    {
        Dictionary<string, byte[]> decodedContentResult = [];

        List<string> fileNames = command.FileEntries.Select(x => x.FileName).ToList();

        (List<EncodingFile> existingFiles, List<EncodingTable> existingTables) = await GetEncodingRelatedDataAsync(fileNames);

        foreach (var fileEntry in command.FileEntries)
        {
            EncodingFile? existingFile = existingFiles.FirstOrDefault(x => x.FileName == fileEntry.FileName);

            if (existingFile is not null)
            {
                EncodingTable? existingTable = existingTables.FirstOrDefault(x => x.Id == existingFile.EncodingTableId);

                if (existingTable is not null)
                {
                    EncodingAlgorithm? algorithm = EncodingAlgorithm.FromValue(existingTable.EncodingAlgorithmId);

                    EncodingLanguage? language = existingTable.EncodingLanguageId.HasValue ? EncodingLanguage.FromValue(existingTable.EncodingLanguageId.Value) 
                                                                                           : null;

                    var decodedContent = DecodeFile
                    (
                        existingTable.EncodedContentBytes,
                        existingTable.EncodingTableElements,
                        algorithm,
                        language
                    );

                    decodedContentResult.Add(existingFile.FileName, Encoding.UTF8.GetBytes(decodedContent));
                }

                continue;
            }
        }

        return await ZipResult(decodedContentResult);
    }
}

internal sealed class DecodeByFileNameCommandHandler : BaseEncoding, ICommandHandler<DecodeByFileNameCommand, Result<byte[]>>
{
    public DecodeByFileNameCommandHandler
    (
        ArchivEaseContext context,
        EncodingAnalyzer analyzer
    ) : base(context, analyzer)
    {
    }

    public async Task<Result<byte[]>> HandleAsync(DecodeByFileNameCommand command, CancellationToken cancellationToken = default)
    {
        Dictionary<string, byte[]> decodedContentResult = [];

        List<string> fileNames = new List<string>()
        {
            command.FileName
        };

        (List<EncodingFile> existingFiles, List<EncodingTable> existingTables) = await GetEncodingRelatedDataAsync(fileNames);

        EncodingFile? existingFile = existingFiles.FirstOrDefault(x => x.FileName == command.FileName);

        if (existingFile is not null)
        {
            EncodingTable? existingTable = existingTables.FirstOrDefault(x => x.Id == existingFile.EncodingTableId);

            if (existingTable is not null)
            {
                EncodingAlgorithm? algorithm = EncodingAlgorithm.FromValue(existingTable.EncodingAlgorithmId);
                EncodingLanguage? language = existingTable.EncodingLanguageId.HasValue ? EncodingLanguage.FromValue(existingTable.EncodingLanguageId.Value)
                                                                                       : null;

                var decodedContent = DecodeFile
                (
                    existingTable.EncodedContentBytes,
                    existingTable.EncodingTableElements,
                    algorithm,
                    language
                );

                decodedContentResult.Add(existingFile.FileName, Encoding.UTF8.GetBytes(decodedContent));
            }
        }

        return await ZipResult(decodedContentResult);
    }
}
