using Rideshare.Domain.Entities;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Persistence.Repositories;
public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(RideshareDbContext dbContext) : base(dbContext)
    {      
    }
}
