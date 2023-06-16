using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Repositories;

public class RateRepository : GenericRepository<RateEntity>, IRateRepository
{
    public RateRepository(RideshareDbContext dbContext) : base(dbContext)
    {
    }
}
