using System.Linq.Expressions;

namespace c4_UnitOfWork.DataAccess;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<Customer, bool>>? filter = null);
    Task AddAsync(TEntity item);
    Task UpdateAsync(TEntity item);
    Task DeleteAsync(TEntity item);
}