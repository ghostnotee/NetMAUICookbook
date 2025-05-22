using System.Collections;

namespace c4_WebApi.DataAccess;

public class CollectionCacheUpdate(string collectionName, Action<IList> updateAction)
{
    public string CollectionKey { get; set; } = collectionName;
    public Action<IList> UpdateAction { get; set; } = updateAction;
}