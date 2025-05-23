using System.Linq.Expressions;

namespace c4_WebApiMauiClient.DataAccess;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity item);
    Task UpdateAsync(TEntity item);
    Task DeleteAsync(TEntity item);
}