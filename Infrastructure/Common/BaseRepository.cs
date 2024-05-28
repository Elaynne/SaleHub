using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Common
{
    public class BaseRepository
    {
        private readonly IMemoryCache _memoryCache;
        public BaseRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void AppendDataOnCache<T>(T input, string cacheKey, Guid id, int expirationTimeInMinutes)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out Dictionary<Guid, T> data))
                data = new Dictionary<Guid, T>();
            data.Add(id, input);

            _memoryCache.Set(cacheKey, data, TimeSpan.FromMinutes(expirationTimeInMinutes));
        }
        public void UpdateDataOnCache<T>(T data, Guid id, 
            string cacheKey, int expirationTimeInMinutes)
        {
            var dataSet = GetDataSet<T>(cacheKey);
            dataSet[id] = data;

            _memoryCache.Set(cacheKey, data, TimeSpan.FromMinutes(expirationTimeInMinutes));
        }
        public Dictionary<Guid, T> GetDataSet<T>(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out Dictionary<Guid, T> cachedData);

            return cachedData;
        }

        public void DeleteItemOnCache<T>(Guid id,
           string cacheKey, int expirationTimeInMinutes)
        {
            var dataSet = GetDataSet<T>(cacheKey);
            dataSet.Remove(id);

            _memoryCache.Set(cacheKey, dataSet, TimeSpan.FromMinutes(expirationTimeInMinutes));
        }
    }
}
