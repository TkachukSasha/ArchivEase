namespace SharedKernel.Caching;

public interface IMemoryCacheService
{
    Task<TEntity> GetOrCreateAsync<TEntity>
    (
       string key,
       TimeSpan? expiry,
       Func<Task<TEntity>> factory
    );

    Task<IReadOnlyCollection<TEntity?>> GetOrCreateEntitiesAsync<TEntity>
    (
       string key,
       TimeSpan? expiry,
       Func<Task<IReadOnlyCollection<TEntity?>>> factory
    );

    void RemoveKey
    (
       string key
    );

    void RemoveByPrefix
    (
       string prefix
    );
}