using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
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
    public async Task<int> GetNoVehicle(int days)
    {
        DateTime startDate = DateTime.Now.AddDays(-days);
        int numberOfVehicles = await _dbContext.Vehicles
            .Where(v => v.DateCreated >= startDate)
            .CountAsync();

        return numberOfVehicles;
    }
}
