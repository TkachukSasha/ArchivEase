using Core.Dal;
using Core.Dtos;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Dal;
using SharedKernel.Queries;
using SharedKernel.Queries.Paging;

namespace Core.Queries;

public class GetFilesQuery : PagedQuery<FileDto>
{
}

internal sealed class GetFilesQueryHandler : IQueryHandler<GetFilesQuery, Paged<FileDto>>
{
    private readonly ArchivEaseContext _context;

    public GetFilesQueryHandler
    (
        ArchivEaseContext context
    ) => _context = context;

    public async Task<Paged<FileDto>> HandleAsync(GetFilesQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<FileDto> files = _context
                      .EncodingFiles
                      .AsNoTracking()
                      .Select
                      (
                        x => 
                            new FileDto
                            (
                                x.FileName, 
                                x.DefaultSize,
                                x.EncodedSize,
                                x.FilePath,
                                x.EncodedFileUnitsOfMeasurement,
                                x.DefaultFileUnitsOfMeasurement
                            )
                      );

        return await files.PaginateAsync(query, cancellationToken);
    }
}
