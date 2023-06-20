using Microsoft.EntityFrameworkCore;
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


         public async Task<List<Driver>> GetDriversWithDetails()
        {
            var drivers =  await _dbContext.Drivers.Include(driver => driver.User).ToListAsync();

            return drivers;
        }

        public async Task<Driver> GetDriverWithDetails(int id)
        {
            var driver = await _dbContext.Drivers.Include(driver => driver.User).FirstOrDefaultAsync(driver => driver.Id == id); 
            return driver;
        }


        
    }
}
