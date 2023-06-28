using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Persistence
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        public Task<Driver> GetDriverWithDetails(int id);
        public Task<PaginatedResponse<Driver>> GetDriversWithDetails(int PageNumber, int PageSize);
         public Task<Driver> GetDriverWithDetailsByUser(string id);
        public Task<Dictionary<int, int>> GetDriversStatistics(string timeframe, int _year = 0, int _month = 0);


        public Task<Driver> GetDriverByUserId(string userId);
        public Task<List<int>> GetCountByStatus ();
    }
}
