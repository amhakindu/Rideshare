using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Repositories;

public class RideRequestRepository : GenericRepository<RideRequest>, IRideRequestRepository
{
    public RideRequestRepository(RideshareDbContext dbContext) : base(dbContext)
    {
    }
}
