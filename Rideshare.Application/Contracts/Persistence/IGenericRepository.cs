using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Contracts.Persistence;


public interface IGenericRepository<T> where T : class
{
    Task<T?> Get(int id);
    Task<PaginatedResponse<T>> GetAll(int pageNumber=1, int pageSize=10);
    Task<int> Add(T entity);
    Task<bool> Exists(int id);
    Task<int> Update(T entity);
    Task<int> Delete(T entity);
    Task<double> GetLastWeekPercentageChange();
    Task<int> Count();
}
