using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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

         public async Task<Driver> GetDriverWithDetailsByUser(string id)
        {
            var driver = await _dbContext.Drivers.Include(driver => driver.User).FirstOrDefaultAsync(driver => driver.UserId == id);
            return driver;
        }






        public async Task <Dictionary<int, int>> GetDriversStatistics(string timeframe, int _year = 0, int _month = 0)
        {

            var driversCount = new Dictionary<int, int>();

            var drivers = await _dbContext.Drivers.OrderBy(driver => driver.DateCreated).ToListAsync();
            if (timeframe.ToLower() == "weekly")
            {
                for (int i = 1; i <= 5; i++)
                    driversCount.Add(i, 0);


                drivers = drivers.Where(driver => driver.DateCreated.Year == _year && driver.DateCreated.Month == _month).ToList();
            }
            else if (timeframe.ToLower() == "monthly")
            {
                for (int i = 1; i <= 12; i++)
                    driversCount.Add(i, 0);

                drivers = drivers.Where(driver => driver.DateCreated.Year == _year).ToList();
            }
            CultureInfo culture = CultureInfo.CurrentCulture;
            Calendar calendar = culture.Calendar;

            foreach (var driver in drivers)
            {
                var date = driver.DateCreated;
                var year = date.Year;
                var month = date.Month;
                var week = calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);

                

                if (timeframe == "weekly")
                {
                    var startMonth = new DateTime(year: year, month: date.Month, day: 1);
                    var startWeek = calendar.GetWeekOfYear(startMonth, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
                    if (!driversCount.ContainsKey((week - startWeek + 1)))
                    {
                        driversCount[(week - startWeek  + 1)] = 0;
                    }
                    driversCount[(week - startWeek + 1)] += 1;
                }
                else if (timeframe == "monthly")
                {
                    if (!driversCount.ContainsKey(month))
                    {
                        driversCount[month] = 0;
                    }
                    driversCount[month] += 1;
                }
                else if (timeframe == "yearly")
                {
                    if (!driversCount.ContainsKey(year))
                    {
                        driversCount[year] = 0;
                    }
                    driversCount[year] += 1;
                }
            }

            return driversCount;
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
