using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace c4_WebApiMauiClient.DataAccess;

public class CustomerRepository(CrmContext context) : IRepository<Customer>
{
    private readonly DbSet<Customer> _dbSet = context.Set<Customer>();

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        IQueryable<Customer> query = _dbSet;
        return await Task.Run(() => query.ToList());
    }

    public async Task AddAsync(Customer item)
    {
        _dbSet.Add(item);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Customer item)
    {
        _dbSet.Remove(item);
        await Task.CompletedTask;
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        return await Task.Run(() => _dbSet.Find(id));
    }

    public async Task UpdateAsync(Customer item)
    {
        _dbSet.Attach(item);
        context.Entry(item).State = EntityState.Modified;
        await Task.CompletedTask;
    }
}