using Domain.Cache;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Infrastructure.Cache
{
    public class CacheService<T> : ICacheService<T>
    {
        private readonly IMemoryCache _memoryCache;
        private const int ExpirationTimeInMinutes = 60;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// Update each record on cache, then update de list of records simulating a DB and rollback on failure.
        /// </summary>
        /// <param name="itemIds"></param>
        /// <param name="fetchItemFunc"></param>
        /// <param name="fetchFinalListFunc"></param>
        /// <param name="finalListKey"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCacheAsync<T>(List<Guid> itemIds, Func<Guid, Task<T>> fetchItemFunc,
            Func<Task<Dictionary<Guid, T>>> fetchFinalListFunc, string finalListKey, string domainKey)
        {
            // Backup original state
           
            _memoryCache.TryGetValue($"{finalListKey}", out Dictionary<Guid, T> originalItems);
             
            var continueUpdate = await UpdateIndividualItem<T>(itemIds, fetchItemFunc, 
                domainKey, originalItems).ConfigureAwait(false);

            if (!continueUpdate)
                return false;

            return await UpdateFinalList<T>(fetchFinalListFunc, finalListKey, originalItems, domainKey)
                .ConfigureAwait(false);
        }

        private async Task<bool> UpdateIndividualItem<T>(List<Guid> itemIds, Func<Guid, Task<T>> fetchItemFunc,
            string domainKey, Dictionary<Guid, T> originalItems)
        {
            foreach (var itemId in itemIds)
            {
                try
                {
                    var item = await fetchItemFunc(itemId);
                    _memoryCache.Set($"{domainKey}_{itemId}", item, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
                }
                catch
                {
                    Rollback<T>(originalItems, domainKey);
                    return false;
                }
            }
            return true;
        }

        private async Task<bool> UpdateFinalList<T>(Func<Task<Dictionary<Guid, T>>> fetchFinalListFunc,
            string finalListKey, Dictionary<Guid, T> originalItems, string domainKey)
        {
            try
            {
                var finalList = await fetchFinalListFunc();
                var originalItemsCopy = originalItems;

                foreach (var item in finalList)
                {
                    originalItemsCopy[item.Key] = item.Value;
                }

                _memoryCache.Set(finalListKey, originalItemsCopy, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
                return true;
            }
            catch
            {
                Rollback<T>(originalItems, domainKey);
                return false;
            }
           
        }

        private void Rollback<T>(Dictionary<Guid, T> originalItems, string domainKey)
        {
            foreach (var item in originalItems)
            {
                _memoryCache.Set($"{domainKey}_{item.Key}", item.Value, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
            }
        }
    }

}
