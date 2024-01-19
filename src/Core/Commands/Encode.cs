using Core.Dal;
using SharedKernel.Commands;
using SharedKernel.Errors;

namespace Core.Commands;

public record EncodeCommand
(
    string FileName,
    string FileUnitsOfMeasurement,
    byte DefaultSize,
    Stream Stream
) : ICommand<Result>;

public record class EncodeCommandHandler : ICommandHandler<EncodeCommand, Result>
{
    private readonly ArchivEaseContext _context;

    public EncodeCommandHandler
    (
        ArchivEaseContext context
    )
    {
        _context = context;
    }

    public Task<Result> HandleAsync(EncodeCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
