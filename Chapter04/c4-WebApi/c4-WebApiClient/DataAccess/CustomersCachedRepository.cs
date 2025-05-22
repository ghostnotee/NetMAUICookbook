using System.Linq.Expressions;

namespace c4_WebApi.DataAccess;

public class CustomersCachedRepository(IRepository<Customer> innerRepository, ICacheService cacheService) : IRepository<Customer>
{
    private const string CollectionName = "customers";

    public async Task<Customer> GetByIdAsync(int id)
    {
        return await innerRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(Expression<Func<Customer, bool>>? filter = null)
    {
        if (!cacheService.TryGetValue(CollectionName, out var customers))
        {
            customers = await innerRepository.GetAllAsync();
            cacheService.Set(CollectionName, customers);
        }

        return (IEnumerable<Customer>)customers!;
    }

    public async Task AddAsync(Customer item)
    {
        await innerRepository.UpdateAsync(item);
        cacheService.AddPendingAction(new CollectionCacheUpdate(CollectionName, cachedList => cachedList.Add(item)));
    }

    public async Task UpdateAsync(Customer item)
    {
        await innerRepository.UpdateAsync(item);
        cacheService.AddPendingAction(new CollectionCacheUpdate(CollectionName, cachedList =>
        {
            var editedItemIndex = ((List<Customer>)cachedList).FindIndex(c => c.Id == item.Id);
            cachedList[editedItemIndex] = item;
        }));
    }

    public async Task DeleteAsync(Customer item)
    {
        await innerRepository.DeleteAsync(item);
        cacheService.AddPendingAction(new CollectionCacheUpdate(CollectionName, cachedList => cachedList.Remove(item)));
    }
}