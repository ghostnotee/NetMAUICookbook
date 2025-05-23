namespace c4_WebApiMauiClient.DataAccess;

public class CrmUnitOfWork : IDisposable, IUnitOfWork<Customer>
{
    readonly ICacheService CacheService = MemoryCacheService.Instance;
    private readonly CrmContext _context = new();
    IRepository<Customer> customerRepository;

    public void Dispose()
    {
    }

    public IRepository<Customer> Items =>
        customerRepository ??= new CustomersCachedRepository(new
            CustomerWebRepository(), CacheService);

    public async Task SaveAsync()
    {
        CacheService.ExecuteCacheUpdateActions();
        await Task.CompletedTask;
    }
}

public interface IUnitOfWork<TEntity> where TEntity : class
{
    IRepository<TEntity> Items { get; }
    Task SaveAsync();
}