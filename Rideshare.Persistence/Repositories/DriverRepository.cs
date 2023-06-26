using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Common.Constants;
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

        




        public async Task<Dictionary<int, Dictionary<string, int>>> GetDriversStatistics(string timeframe)
        {
            Dictionary<int, Dictionary<string, int>> driversCount = new Dictionary<int, Dictionary<string, int>>();

            var drivers = await _dbContext.Drivers.OrderBy(driver => driver.DateCreated).ToListAsync();
            CultureInfo culture = CultureInfo.CurrentCulture;
            Calendar calendar = culture.Calendar;

            foreach (var driver in drivers)
            {
                var date = driver.DateCreated;
                var year = date.Year;
                var month = date.ToString("MMMM");
                var week = calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);

                if (!driversCount.ContainsKey(year))
                {
                    driversCount[year] = new Dictionary<string, int>();
                }

                if (timeframe == "weekly")
                {
                    if (!driversCount[year].ContainsKey(week.ToString()))
                    {
                        driversCount[year][week.ToString()] = 0;
                    }
                    driversCount[year][week.ToString()] += 1;
                }
                else if (timeframe == "monthly")
                {
                    if (!driversCount[year].ContainsKey(month))
                    {
                        driversCount[year][month] = 0;
                    }
                    driversCount[year][month] += 1;
                }
                else if (timeframe == "yearly")
                {
                    if (!driversCount[year].ContainsKey(year.ToString()))
                    {
                        driversCount[year][year.ToString()] = 0;
                    }
                    driversCount[year][year.ToString()] += 1;
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
