using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Persistence.Repositories;

public class RideRequestRepository : GenericRepository<RideRequest>, IRideRequestRepository
{
    private readonly RideshareDbContext _dbContext;
    
    public RideRequestRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Dictionary<string, int>> GetTop5Commuter()
    {
        var topUsers = await _dbContext.Set<RideRequest>()
            .GroupBy(rideRequest => rideRequest.UserId)
            .Select(group => new
            {
                UserId = group.Key,
                RideRequestCount = group.Count()
            })
            .OrderByDescending(user => user.RideRequestCount)
            .Take(5)
            .ToDictionaryAsync(user => user.UserId, user => user.RideRequestCount);
        return topUsers;
    }
}
