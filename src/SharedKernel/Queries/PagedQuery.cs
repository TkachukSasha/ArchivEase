using SharedKernel.Queries.Paging;

namespace SharedKernel.Queries;

public abstract class PagedQuery : IPagedQuery
{
    public int Page { get; set; }
    public int Results { get; set; }
}

public abstract class PagedQuery<T> : PagedQuery, IPagedQuery<Paged<T>>
{
}