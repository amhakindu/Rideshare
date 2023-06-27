using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using static Rideshare.Application.Common.Constants.Utils;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Persistence.Repositories;

public class RideRequestRepository : GenericRepository<RideRequest>, IRideRequestRepository
{
    private const double RADIUS_IN_METERS = 150.0;
    private readonly RideshareDbContext _dbContext;
    
    public RideRequestRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RideRequest?> Get(int id)
    {
        return _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Include(ro => ro.Origin)
            .Include(ro => ro.Destination)
            .FirstOrDefault(ro => ro.Id == id);
    }

    public async Task<int> Add(RideRequest entity)
    {
        var locations = await _dbContext.Locations.ToListAsync();
        var temp1 = locations.OrderBy(g => HaversineDistance(g.Coordinate, entity.Origin.Coordinate))
            .FirstOrDefault();
        var temp2 = locations.OrderBy(g => HaversineDistance(g.Coordinate, entity.Destination.Coordinate))
            .FirstOrDefault();
        
        if (temp1 != null && HaversineDistance(temp1.Coordinate, entity.Origin.Coordinate) <= RADIUS_IN_METERS)
            entity.Origin = null;
        if (temp2 != null && HaversineDistance(temp2.Coordinate, entity.Destination.Coordinate) <= RADIUS_IN_METERS)
            entity.Destination = null;

        await _dbContext.AddAsync(entity);

        if(temp1 != null)
            entity.Origin = temp1;
        if(temp2 != null)
            entity.Destination = temp2;
        await _dbContext.SaveChangesAsync();
        return await Update(entity);
    }

    public async Task<IReadOnlyList<RideRequest>> GetAll(int pageNumber=1, int pageSize=10)
    {
        return await _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(ro => ro.Origin)
            .Include(ro => ro.Destination)
            .ToListAsync();
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
    public async Task<Dictionary<int,int>> GetAllByGivenParameter(string? type ,int? year, int? month)
    {
        Dictionary<int,int> rideRequests = new();
        if (type == "monthly"){
           rideRequests =  await _dbContext.Set<RideRequest>().AsNoTracking()
                .GroupBy(item => item.DateCreated.Month)
                .ToDictionaryAsync(g => g.Key,g => g.Count());
        }
        else if(type == "weekly"){
            rideRequests =  await _dbContext.Set<RideRequest>().AsNoTracking()
                .GroupBy(item => item.DateCreated.Month)
                .ToDictionaryAsync(g => g.Key,g => g.Count());
        }
         else
         {
           rideRequests =  await _dbContext.Set<RideRequest>().AsNoTracking()
                .GroupBy(item => item.DateCreated.Year)
                .ToDictionaryAsync(g => g.Key,g => g.Count());
        }
        return rideRequests;
    }

    public Task<Dictionary<string, int>> GetAllByGivenStatus(string? type, int? year, int? month, Status? status)
    {
        throw new NotImplementedException();
    }



    // public async Task<Dictionary<string, int>> GetAllByGivenStatus(string? type, int? year, int? month, Status? status)
    // {
    //     Dictionary<int,int> rideRequests = new();
    //     if (type == "monthly"){
    //        rideRequests =  await _dbContext.Set<RideRequest>().AsNoTracking()
    //             .GroupBy(item => item.DateCreated.Month)
    //             .ToDictionaryAsync(g => g.Key,g => g.Count());
    //     }
    //     else if(type == "weekly"){
    //         rideRequests =  await _dbContext.Set<RideRequest>().AsNoTracking()
    //             .GroupBy(item => item.DateCreated.Month)
    //             .ToDictionaryAsync(g => g.Key,g => g.Count());
    //     }
    //      else
    //      {
    //        rideRequests =  await _dbContext.Set<RideRequest>().AsNoTracking()
    //             .GroupBy(item => item.DateCreated.Year)
    //             .ToDictionaryAsync(g => g.Key,g => g.Count());
    //     }
    //     // return rideRequests;

    // }

    public async Task<IReadOnlyList<RideRequest>> SearchByGivenParameter(int PageNumber, int PageSize, Status? status, int? fare, string name, string phoneNumber)
    {
         return await _dbContext.Set<RideRequest>().AsNoTracking()
        .Where(item => (item.Status == (status ?? item.Status)))
        .Where(item => (item.User.PhoneNumber == (phoneNumber ?? item.User.PhoneNumber)))
        .Where(item => (item.CurrentFare  <= (fare ?? item.CurrentFare)))
        .Where(item => ( item.User.FullName == (name ?? item.User.FullName)))
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

    }

    public Task<IReadOnlyList<RideRequest>> SearchByGivenParameter(int PageNumber, int PageSize, Status? status, int fare, string name, string phoneNumber)
    {
        throw new NotImplementedException();
    }
}
