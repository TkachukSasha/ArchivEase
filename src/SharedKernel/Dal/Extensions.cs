using SharedKernel.Queries.Paging;
using SharedKernel.Queries;
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Dal;

public static class Extensions
{
    public static async Task<Paged<TEntity>> PaginateAsync<TEntity>
    (
         this IQueryable<TEntity> data,
         IPagedQuery query,
         CancellationToken cancellationToken = default
    )
    {
        if (query.Page <= 0)
            query.Page = 1;

        switch (query.Results)
        {
            case var value when value <= 0:
                query.Results = 10;
                break;
            case var value when value > 100:
                query.Results = 100;
                break;
            default:
                break;
        }

        var totalResults = await data.CountAsync(cancellationToken);

        var totalPages = totalResults <= query.Results ? 1 : (int)Math.Floor((double)totalResults / query.Results);

        var items = data
             .Skip((query.Page - 1) * query.Results)
             .Take(query.Results);

        var result = await items.ToListAsync(cancellationToken);

        var hasPreviousPage = query.Page > 1;

        var hasNextPage = query.Page * query.Results < totalResults;

        return new Paged<TEntity>
        (
            result,
            query.Page,
            query.Results,
            totalPages,
            totalResults,
            hasPreviousPage,
            hasNextPage
        );
    }
}
