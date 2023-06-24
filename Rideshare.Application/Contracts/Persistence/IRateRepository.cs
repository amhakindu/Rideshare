using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRateRepository: IGenericRepository<RateEntity>
{
    public Task<List<RateEntity>> GetRatesByDriverId (int pageNumber, int pageSize, int driverId);
}
