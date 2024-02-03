using Core.Dal;
using Core.Dtos;
using SharedKernel.Commands;
using SharedKernel.Errors;
using SharedKernel.Files;
namespace Core.Commands;

public record DecodeCommand
(
    List<FileEntryDto> FileEntries
) : ICommand<Result>;

internal sealed class DecodeCommandHandler : ICommandHandler<DecodeCommand, Result>
{
    private readonly ArchivEaseContext _context;
    private readonly IFileGetter _fileGetter;

    public DecodeCommandHandler
    (
        ArchivEaseContext context,
        IFileGetter fileGetter
    )
    {
        _context = context;
        _fileGetter = fileGetter;
    }

    public async Task<Result> HandleAsync(DecodeCommand command, CancellationToken cancellationToken = default)
    {
        foreach (var file in command.FileEntries)
            _ = await _fileGetter.GetFileBytesAsync(file.FileName);

        return Result.Success();
    }
}
