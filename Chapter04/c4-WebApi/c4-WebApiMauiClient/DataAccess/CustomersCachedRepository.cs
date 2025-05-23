using System.Linq.Expressions;

namespace c4_WebApiMauiClient.DataAccess;

public class CustomersCachedRepository(IRepository<Customer> innerRepository, ICacheService cacheService) : IRepository<Customer>
{
    protected readonly ICacheService CacheService = cacheService;
    protected readonly string CollectionName = "customers";
    protected readonly IRepository<Customer> InnerRepository = innerRepository;

    public async Task<Customer> GetByIdAsync(int id)
    {
        return await InnerRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(Expression<Func<Customer, bool>>? filter = null)
    {
        if (!CacheService.TryGetValue(CollectionName, out var customers))
        {
            customers = await InnerRepository.GetAllAsync();
            CacheService.Set(CollectionName, customers);
        }

        return (IEnumerable<Customer>)customers!;
    }

    public async Task AddAsync(Customer item)
    {
        await InnerRepository.AddAsync(item);
        CacheService.AddPendingAction(new CollectionCacheUpdate(CollectionName, cachedList => cachedList.Add(item)));
    }

    public async Task UpdateAsync(Customer item)
    {
        await InnerRepository.UpdateAsync(item);
        CacheService.AddPendingAction(new CollectionCacheUpdate(CollectionName, cachedList =>
        {
            var editedItemIndex = ((List<Customer>)cachedList).FindIndex(c => c.Id == item.Id);
            cachedList[editedItemIndex] = item;
        }));
    }

    public async Task DeleteAsync(Customer item)
    {
        await InnerRepository.DeleteAsync(item);
        CacheService.AddPendingAction(new CollectionCacheUpdate(CollectionName, cachedList => cachedList.Remove(item)));
    }
}