namespace c4_DataCaching.DataAccess;

public interface ICacheService
{
    bool TryGetValue(object key, out object? result);
    TItem Set<TItem>(object key, TItem value);
    void ClearCollectionCache(string collectionKey);
    void AddPendingAction(CollectionCacheUpdate pendingAction);
    void ClearCacheUpdateActions();
    void ExecuteCacheUpdateActions();
}