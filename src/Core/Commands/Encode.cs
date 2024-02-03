using Core.Dal;
using Core.Dtos;
using Core.Encodings;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Commands;
using SharedKernel.Errors;
using SharedKernel.Files;

namespace Core.Commands;

public record EncodeCommand
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
        List<FileDto> files = new();

        foreach(var fileEntry in command.FileEntries)
        {
            EncodingFile? existingFile = await _context
                .EncodingFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FileName == fileEntry.FileName);

            if(existingFile is not null)
            {
                AddExistingFileInformationToResult(files, existingFile);

                continue;
            }

            Result<string> pathValue = await _fileSetter.SetFileAsync(fileEntry.Stream, fileEntry.FileName);

            string unitsOfMeasurement = _fileGetter.GetFileUnitsOfMeasurement(fileEntry.FileLength);
        }

        return Result.Success();
    }

    private void AddExistingFileInformationToResult
    (
        List<FileDto> files,
        EncodingFile existingFile
    )
    {
        files.Add
           (
               new FileDto
               (
                   existingFile.FileName,
                   existingFile.DefaultSize,
                   existingFile.EncodedSize,
                   existingFile.FilePath,
                   existingFile.FileUnitsOfMeasurement
               )
           );
    }
}
