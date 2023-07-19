using System.Globalization;
using Rideshare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Persistence.Repositories
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        private readonly RideshareDbContext _dbContext;
        public DriverRepository(RideshareDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Driver> GetDriverByUserId(string userId)
        {
            var driver = await _dbContext.Drivers.Include(driver => driver.User).FirstOrDefaultAsync(driver => driver.UserId == userId);
            return driver;

        }

        public async Task<PaginatedResponse<Driver>> GetDriversWithDetails(int pageNumber, int pageSize)
        {
            var response = new PaginatedResponse<Driver>();
            var driversQuery = _dbContext.Drivers.Include(driver => driver.User);
            var count = await driversQuery.CountAsync();
            var drivers = await driversQuery.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            response.Count = count;
            response.Value = drivers;
            return response;
        }

        public async Task<Driver> GetDriverWithDetails(int id)
        {
            var driver = await _dbContext.Drivers.Include(driver => driver.User).FirstOrDefaultAsync(driver => driver.Id == id);
            return driver;
        }

         public async Task<Driver> GetDriverWithDetailsByUser(string id)
        {
            var driver = await _dbContext.Drivers.Include(driver => driver.User).FirstOrDefaultAsync(driver => driver.UserId == id);
            return driver;
        }

        public async Task<List<int>> GetCountByStatus()
        {

            List<int> count = new List<int>();
            DateTime startDate = DateTime.Now.AddDays(-30);
            var all = _dbContext.Drivers.Include(driver => driver.User);

            var actives = all.Where(driver => driver.User.LastLogin >= startDate);

            count.Add(actives.Count());
            count.Add(all.Count() - actives.Count());
            return count;
            
        }
    }
}
