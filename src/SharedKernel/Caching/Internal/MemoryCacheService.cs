using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace SharedKernel.Caching.Internal;

internal sealed class MemoryCacheService : IMemoryCacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(3);

    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new ConcurrentDictionary<string, bool>();

    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService
    (
        IMemoryCache memoryCache
    )
    {
        _memoryCache = memoryCache;
    }

    public async Task<TEntity> GetOrCreateAsync<TEntity>
    (
       string key,
       TimeSpan? expiry,
       Func<Task<TEntity>> factory
    )
    {
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
        TEntity result = await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(expiry ?? DefaultExpiration);

                CacheKeys.TryAdd(key, false);

                return factory();
            });
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.

#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
        return result;
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
    }

    public async Task<IReadOnlyCollection<TEntity?>> GetOrCreateEntitiesAsync<TEntity>
    (
       string key,
       TimeSpan? expiry,
       Func<Task<IReadOnlyCollection<TEntity?>>> factory
    )
    {
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
        IReadOnlyCollection<TEntity?> result = await _memoryCache.GetOrCreateAsync(
          key,
          entry =>
          {
              entry.SetAbsoluteExpiration(expiry ?? DefaultExpiration);

              CacheKeys.TryAdd(key, false);

              return factory();
          });
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.

#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
        return result;
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
    }

    public void RemoveKey(string key)
    {
        if (CacheKeys.TryGetValue(key, out var _))
        {
            _memoryCache.Remove(key);

            CacheKeys.TryRemove(key, out _);
        }
    }

    public void RemoveByPrefix(string prefix)
    {
        var keysToRemove = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefix));

        foreach (var key in keysToRemove)
        {
            _memoryCache.Remove(key);
            CacheKeys.TryRemove(key, out _);
        }
    }
}