namespace Rideshare.Application.Contracts.Persistence;


public interface IGenericRepository<T> where T : class
{
    Task<T?> Get(int id);
    Task<IReadOnlyList<T>> GetAll(int PageNumber, int PageSize);
    Task<int> Add(T entity);
    Task<bool> Exists(int id);
    Task<int> Update(T entity);
    Task<int> Delete(T entity);
}
