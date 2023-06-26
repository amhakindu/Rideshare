using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Repositories;

public class RateRepository : GenericRepository<RateEntity>, IRateRepository
{
    private readonly RideshareDbContext _dbContext;

    public RateRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RateEntity>> GetRatesByDriverId(int pageNumber, int pageSize, int driverId)
    {

      var rates =  await _dbContext.RateEntities.Where(rate => rate.DriverId == driverId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    
    return rates;
    }
}
