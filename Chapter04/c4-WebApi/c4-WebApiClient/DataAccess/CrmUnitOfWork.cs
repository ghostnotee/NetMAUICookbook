namespace c4_WebApi.DataAccess;

public class CrmUnitOfWork : IDisposable, IUnitOfWork<Customer>
{
    private readonly ICacheService _cacheService = MemoryCacheService.Instance;
    private readonly CrmContext _context = new();
    private IRepository<Customer>? _customerRepository;

    public void Dispose()
    {
    }

    public IRepository<Customer> Items => _customerRepository ??= new CustomersCachedRepository(new CustomerWebRepository(), _cacheService);

    public async Task SaveAsync()
    {
        _cacheService.ExecuteCacheUpdateActions();
        await Task.CompletedTask;
    }
}

public interface IUnitOfWork<TEntity> where TEntity : class
{
    IRepository<TEntity> Items { get; }
    Task SaveAsync();
}