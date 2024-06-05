using Core.Commands.Base;
using Core.Dal;
using Core.Dtos;
using Core.Encodings;
using Core.Encodings.Builders.ShannonFano;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Commands;
using SharedKernel.Errors;
using SharedKernel.Files;
using System.IO.Compression;
using System.Text;

namespace Core.Commands;

public sealed record EncodeCommand
(
   Guid UserId,
   List<FileEntryDto> FileEntries
) : ICommand<Result<byte[]>>;

internal sealed class EncodeCommandHandler : BaseEncoding, ICommandHandler<EncodeCommand, Result<byte[]>>
{
    private static object _lock = new object();
    private readonly IFileSetter _fileSetter;
    private readonly IFileGetter _fileGetter;

    public EncodeCommandHandler
    (
        ArchivEaseContext context,
        IFileSetter fileSetter,
        IFileGetter fileGetter,
        EncodingAnalyzer analyzer
    ) : base(context, analyzer)
    {
        _fileSetter = fileSetter;
        _fileGetter = fileGetter;
    }

    public async Task<Result<byte[]>> HandleAsync(EncodeCommand command, CancellationToken cancellationToken = default)
    {
        #region fields
        Dictionary<string, byte[]> encodedContentResult = [];

        List<EncodingTable> _encodingTables = [];

        List<EncodingFile> _encodingFiles = [];

        List<string> fileNames = command.FileEntries.Select(x => x.FileName).ToList();
        #endregion

        (List<EncodingFile> existingFiles, List<EncodingTable> existingTables) = await GetEncodingRelatedDataAsync(fileNames);

        foreach (var fileEntry in command.FileEntries)
        {
            EncodingFile? existingFile = existingFiles.FirstOrDefault(x => x.FileName == fileEntry.FileName);

            if (existingFile is not null)
            {
                EncodingTable? existingTable = existingTables.FirstOrDefault(x => x.Id == existingFile.EncodingTableId);

                if (existingTable is not null)
                {
                    encodedContentResult.Add(existingFile.FileName, existingTable.EncodedContentBytes);
                }

                continue;
            }

            (byte[] encodedData, EncodingTableElements? encodingTableElements, Guid languageId, Guid algorithmId) = await EncodeFileAsync(fileEntry.Stream);

            Result<FileInfoDto> fileInfo = await SaveEncodedFileAsync(encodedData, fileEntry.FileName);

            lock (_lock)
            {
                SetEntities
                (
                    fileEntry,
                    fileInfo.Value,
                    encodedData,
                    encodingTableElements,
                    _encodingTables,
                    _encodingFiles,
                    languageId,
                    algorithmId,
                    command.UserId
                );

                encodedContentResult.Add(fileEntry.FileName, encodedData);
            }
        }

        _context.EncodingTables.AddRange(_encodingTables);

        _context.EncodingFiles.AddRange(_encodingFiles);

        return await ZipResult(encodedContentResult);
    }

    private async Task<Result<FileInfoDto>> SaveEncodedFileAsync
    (
        byte[] encodedData,
        string fileName
    )
    {
        using (var encodedStream = new MemoryStream(encodedData))
        {
            return await _fileSetter.SetFileAsync(encodedStream, fileName);
        }
    }

    private void SetEntities
    (
        FileEntryDto fileEntry,
        FileInfoDto fileInfo,
        byte[] encodedData,
        EncodingTableElements encodingTableElements,
        List<EncodingTable> encodingTables,
        List<EncodingFile> encodingFiles,
        Guid languageId,
        Guid algorithmId,
        Guid userId
    )
    {
        (double fileLength, string unitsOfMeasurement) = FileExtensions.GetFileSizeUnitsOfMeasurement(fileEntry.Length);

        EncodingTable encodingTable = EncodingTable.Init
        (
            encodedData,
            languageId,
            algorithmId,
            encodingTableElements
        ).Value;

        EncodingFile encodingFile = EncodingFile.Init
        (
            encodingTable.Id,
            fileInfo.FilePath,
            fileEntry.FileName,
            fileInfo.EncodedUnitsOfMeasurement,
            unitsOfMeasurement,
            fileEntry.ContentType,
            fileInfo.FileLength,
            fileLength,
            userId
        ).Value;

        encodingTables.Add(encodingTable);
        encodingFiles.Add(encodingFile);
    }
}

