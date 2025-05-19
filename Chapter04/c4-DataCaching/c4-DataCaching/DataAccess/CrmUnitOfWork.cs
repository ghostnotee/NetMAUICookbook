namespace c4_DataCaching.DataAccess;

public class CrmUnitOfWork : IDisposable, IUnitOfWork<Customer>
{
    private readonly CrmContext _context = new();
    private IRepository<Customer>? _customerRepository;

    public void Dispose()
    {
        _context.Dispose();
    }

    public IRepository<Customer> Items => _customerRepository ??= new CustomerRepository(_context);

    public async Task SaveAsync()
    {
        await Task.Run(() => _context.SaveChanges());
    }
}

public interface IUnitOfWork<TEntity> where TEntity : class
{
    IRepository<TEntity> Items { get; }
    Task SaveAsync();
}