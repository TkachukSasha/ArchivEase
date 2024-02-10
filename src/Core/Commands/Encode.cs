using Core.Dal;
using Core.Dtos;
using Core.Encodings;
using Core.Encodings.Builders.Huffman;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Commands;
using SharedKernel.Errors;
using SharedKernel.Files;
using System.Text;

namespace Core.Commands;

public sealed record EncodeCommand
(
   List<FileEntryDto> FileEntries
) : ICommand<Result>;

internal sealed class EncodeCommandHandler : ICommandHandler<EncodeCommand, Result>
{
    private readonly ArchivEaseContext _context;
    private readonly IFileSetter _fileSetter;
    private readonly IFileGetter _fileGetter;

    public EncodeCommandHandler
    (
        ArchivEaseContext context,
        IFileSetter fileSetter,
        IFileGetter fileGetter
    )
    {
        _context = context;
        _fileSetter = fileSetter;
        _fileGetter = fileGetter;
    }

    public async Task<Result> HandleAsync(EncodeCommand command, CancellationToken cancellationToken = default)
    {
        List<EncodingTable> _encodingTables = new();

        List<EncodingFile> _encodingFiles = new();

        // check that the file already exists
        List<string> fileNames = command.FileEntries.Select(x => x.FileName).ToList();

        List<EncodingFile> existingFiles = await _context
            .EncodingFiles
            .AsNoTracking()
            .Where(x => fileNames.Contains(x.FileName))
            .ToListAsync(cancellationToken);

        // send to ml request to predict algo
        string predictedAlgorithm = EncodingAlgorithm.HuffmanAlgorithm.Name;

        // encode by algo && add to result list
        foreach (var fileEntry in command.FileEntries)
        {
            if (existingFiles.FirstOrDefault(x => x.FileName == fileEntry.FileName) is not null)
            {
                // get from storage && add to result

                continue;
            }

            string content;

            using (var reader = new StreamReader(fileEntry.Stream, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();

                (byte[] encodedData, EncodingTableElements encodingTableElements) = HuffmanEncodeBuilder
                    .Init()
                    .WithContent(content)
                    .WithTableElements(null)
                    .PrepareContent()
                    .Build();

                (double fileLength, string unitsOfMeasurement) = _fileGetter.GetFileSizeUnitsOfMeasurement(fileEntry.Length);

                using (var encodedStream = new MemoryStream(encodedData))
                {
                    Result<FileInfoDto> fileInfo = await _fileSetter.SetFileAsync(encodedStream, fileEntry.FileName);

                    (double encodedFileLength, string encodedUnitsOfMeasurement) = _fileGetter.GetFileSizeUnitsOfMeasurement(fileInfo.Value.FileLength);

                    EncodingTable encodingTable = EncodingTable.Init(encodedData, EncodingAlgorithm.HuffmanAlgorithm.Value, null, encodingTableElements).Value;

                    EncodingFile encodingFile = EncodingFile.Init(encodingTable.Id, fileInfo.Value.FilePath, fileEntry.FileName, encodedUnitsOfMeasurement, unitsOfMeasurement, fileEntry.ContentType, encodedFileLength, fileLength).Value;

                    _encodingTables.Add(encodingTable);
                    _encodingFiles.Add(encodingFile);
                }
            }
        }

        // zip result list

        #region commented

        //if (command.Algorithm == variableLength)
        //{
        //    return Result.Success(VariableLengthEncodeAlgorithm(command.Text, command.Language));
        //}

        //if(command.Algorithm == shannonFano || command.Algorithm == huffman)
        //{
        //    return Result.Success(SharedAlgorithm(command.Text, command.Algorithm == shannonFano));
        //}

        #endregion

        return Result.Success();
    }
}

