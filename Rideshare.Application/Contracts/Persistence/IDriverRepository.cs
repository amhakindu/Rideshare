using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Contracts.Persistence
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        public Task<Driver> GetDriverWithDetails(int id);
        public Task<PaginatedResponse<Driver>> GetDriversWithDetails(int PageNumber, int PageSize);
        public Task<Driver> GetDriverWithDetailsByUser(string id);
        public Task<Driver> GetDriverByUserId(string userId);
        public Task<List<int>> GetCountByStatus ();
    }
}
