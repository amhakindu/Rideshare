using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Persistence.Repositories;
public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    private readonly RideshareDbContext _dbContext;
    public VehicleRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
       
    }
    public async Task<Dictionary<int, int>> GetNoVehicle(string option, int year, int month)
    {
        Dictionary<int, int> result;

        if (option == "monthly")
        {
            result = await GetNoVehicleByMonth(year);
        }
        else if (option == "yearly")
        {
            result = await GetNoVehicleByYear();
        }
        else if (option == "weekly")
        {
            result = await GetNoVehicleByWeek(year, month);
        }
        else
        {
            // Handle invalid option value
            throw new ArgumentException("Invalid option value");
        }

        return result;
    }

    private async Task<Dictionary<int, int>> GetNoVehicleByMonth(int year)
    {
        var vehicleCounts = await _dbContext.Vehicles
            .Where(v => v.DateCreated.Year == year)
            .GroupBy(v => v.DateCreated.Month)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

            for (int month = 1; month <= 12; month++)
            {
                if(!vehicleCounts.ContainsKey(month))
                    vehicleCounts.Add(month, 0);
            }

        return vehicleCounts;
    }

    private async Task<Dictionary<int, int>> GetNoVehicleByYear()
    {
        var vehicleCounts = await _dbContext.Vehicles
            .GroupBy(v => v.DateCreated.Year)
            .Select(g => new
            {
                Year = g.Key,
                Count = g.Count()
            })
            .ToDictionaryAsync(g => g.Year, g => g.Count);

        for (int year = 2023; year <= DateTime.Now.Year; year++)
        {
            if(!vehicleCounts.ContainsKey(year))
                vehicleCounts.Add(year, 0);
        }

        return vehicleCounts;
    }

    private async Task<Dictionary<int, int>> GetNoVehicleByWeek(int year, int month)
    {
        var vehicleCounts = await _dbContext.Vehicles
            .Where(v => v.DateCreated.Year == year && v.DateCreated.Month == month)
            .GroupBy(v => (int)(v.DateCreated.Day / 7) + 1)
            .ToDictionaryAsync(group => group.Key, group =>group.Count());

            for (int week = 1; week <= 5; week++)
            {
                if(!vehicleCounts.ContainsKey(week))
                    vehicleCounts.Add(week, 0);
            }

        return vehicleCounts;
    }
}
