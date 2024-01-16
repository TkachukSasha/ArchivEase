namespace SharedKernel.Queries;

public interface ICachedQuery
{
    string Key { get; }

    TimeSpan? Expiry { get; }
}

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;