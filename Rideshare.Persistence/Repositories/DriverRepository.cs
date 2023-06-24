using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Common.Constants;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<Driver>> GetDriversWithDetails(int pageNumber, int pageSize)
        {
            var drivers = await _dbContext.Drivers.Include(driver => driver.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return drivers;
        }

        public async Task<Driver> GetDriverWithDetails(int id)
        {
            var driver = await _dbContext.Drivers.Include(driver => driver.User).FirstOrDefaultAsync(driver => driver.Id == id);
            return driver;
        }

        public async Task<Dictionary<string, int>> GetDriversStatistics(bool weekly = false, bool monthly = false, bool yearly = true)
        {

            Dictionary<string, int> driversCount = new Dictionary<string, int>();

            var drivers = await _dbContext.Drivers.OrderBy(driver => driver.DateCreated).ToListAsync();
            int count = 0;
            int i = 0;
            var startDate = StartDate.Date;
            var interval = weekly ? 7 : monthly ? 30 : 365;

            while (i < drivers.Count)
            {
                var difference = drivers[i].DateCreated - startDate;
                if (difference.Days > interval)
                {
                    driversCount[startDate.ToString("MMM    ")] = count;
                    startDate = startDate.AddDays(interval);
                    count = 0;

                }
                else
                    count++; i++;

            }

            return driversCount;



        }




    }
}
