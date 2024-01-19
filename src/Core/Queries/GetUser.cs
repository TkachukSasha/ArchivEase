using Core.Dal;
using Core.Dtos;
using Core.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Caching;
using SharedKernel.Queries;

namespace Core.Queries;

public record GetUserQuery(Guid Id) : ICachedQuery<UserDto?>
{
    public string Key => $"user_{Id}";

    public TimeSpan? Expiry => TimeSpan.FromHours(1);
}

public sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto?>
{
    private readonly ArchivEaseContext _context;
    private readonly IMemoryCacheService _memoryCacheService;

    public GetUserQueryHandler
    (
        ArchivEaseContext context,
        IMemoryCacheService memoryCacheService
    )
    {
        _context = context;
        _memoryCacheService = memoryCacheService;
    }

    public async Task<UserDto?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken = default)
        => await _memoryCacheService
                .GetOrCreateAsync
                (
                    query.Key,
                    query.Expiry,
                    async () =>
                    {
                        User? user = await _context
                            .Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

                        return new UserDto(user is not null ? user.Id : Guid.Empty, user is not null ? user.UserName : string.Empty);
                    }
                );
}
