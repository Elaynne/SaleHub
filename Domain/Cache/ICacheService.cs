namespace Domain.Cache
{
    public interface ICacheService<T>
    {
        Task<bool> UpdateCacheAsync<T>(List<Guid> itemIds, Func<Guid, Task<T>> fetchItemFunc,
            Func<Task<Dictionary<Guid, T>>> fetchFinalListFunc, string finalListKey, string itemKey);
    }
}
