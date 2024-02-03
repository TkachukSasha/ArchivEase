using Core.Dal;
using Core.Dtos;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Dal;
using SharedKernel.Queries;
using SharedKernel.Queries.Paging;

namespace Core.Queries;

public class GetUsersQuery : PagedQuery<UserDto>
{
}


internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Paged<UserDto>>
{
    private readonly ArchivEaseContext _context;

    public GetUsersQueryHandler
    (
        ArchivEaseContext context
    ) => _context = context;

    public async Task<Paged<UserDto>> HandleAsync(GetUsersQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<UserDto> users = _context
                       .Users
                       .AsNoTracking()
                       .Select
                       (
                         x => 
                             new UserDto
                             (
                                 x!.Id,
                                 x.UserName
                             )
                       );

        return await users.PaginateAsync(query, cancellationToken);
    }
}