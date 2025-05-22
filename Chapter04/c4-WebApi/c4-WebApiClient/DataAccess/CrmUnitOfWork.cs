namespace c4_WebApi.DataAccess;

public class CrmUnitOfWork : IDisposable, IUnitOfWork<Customer>
{
    private readonly ICacheService _cacheService = MemoryCacheService.Instance;
    private readonly CrmContext _context = new();
    private IRepository<Customer>? _customerRepository;

    public void Dispose()
    {
        _context.Dispose();
    }

    public IRepository<Customer> Items => _customerRepository ??= new CustomersCachedRepository(new CustomerRepository(_context), _cacheService);

    public async Task SaveAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                _cacheService.ClearCacheUpdateActions();
                throw;
            }

            _cacheService.ExecuteCacheUpdateActions();
        });
    }
}

public interface IUnitOfWork<TEntity> where TEntity : class
{
    IRepository<TEntity> Items { get; }
    Task SaveAsync();
}