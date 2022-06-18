namespace DDD.Api.Domain.Interface.Infra.Repositories;
public interface IRepositoryBase<T> where T : new()
{
    Task<List<T>> SelectAllAsync();
    Task<T?> SelectByIdOrDefaultAsync(int id);
    Task<int> SelectNextInsertIdAsync();
    Task InsertAsync(int insertId, T entity);
    Task UpdateAsync(int id, T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}