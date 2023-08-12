using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Persistence.Repositories;

public class RideRequestRepository : GenericRepository<RideRequest>, IRideRequestRepository
{
    private const double RADIUS_IN_KM = 0.015;
    private readonly RideshareDbContext _dbContext;

    public RideRequestRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RideRequest?> Get(int id)
    {
        return _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .FirstOrDefault(rr => rr.Id == id);
    }
    public async Task<int> Add(RideRequest entity)
    {
		Point current= entity.Origin.Coordinate;
		var temp1 = await _dbContext.Locations
			.Where(location => RideshareDbContext.haversine_distance(location.Coordinate.X, location.Coordinate.Y, current.X, current.Y) <= RADIUS_IN_KM)
			.OrderBy(g => RideshareDbContext.haversine_distance(g.Coordinate.X, g.Coordinate.Y, current.X, current.Y))
			.FirstOrDefaultAsync();

		var destination = entity.Destination.Coordinate;
		var temp2 = await _dbContext.Locations
			.Where(location => RideshareDbContext.haversine_distance(location.Coordinate.X, location.Coordinate.Y, destination.X, destination.Y) <= RADIUS_IN_KM)
			.OrderBy(g => RideshareDbContext.haversine_distance(g.Coordinate.X, g.Coordinate.Y, destination.X, destination.Y))
			.FirstOrDefaultAsync();

		if(temp1 != null && temp2 != null && temp1.Equals(temp2))
			throw new ValidationException($"Failed to Create RideRequest! Origin and Detination must be farther than {RADIUS_IN_KM} km");

		entity.Origin = temp1 ?? entity.Origin;
		entity.Destination = temp2 ?? entity.Destination;

		await _dbContext.AddAsync(entity);

		return await _dbContext.SaveChangesAsync();
    }

    public async Task<PaginatedResponse<RideRequest>> GetAllRequests(int pageNumber = 1, int pageSize = 10)
    {
        var response = new PaginatedResponse<RideRequest>();

        var rideRequests = await _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.User)
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .ToListAsync();
        response.Count = await _dbContext.RideRequests.CountAsync();
        response.Value = rideRequests;
        return response;
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

    public Task<List<GeographicalLocation>> GetPopularDestinationsOfCommuter(string UserId, int limit)
    {
        return _dbContext.Set<RideRequest>()
            .Where(riderequest => riderequest.UserId == UserId)
            .GroupBy(riderequest => riderequest.Destination)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<RideRequest?> GetRideRequestWithDetail(int riderequestId)
    {
        return await _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .Include(rr => rr.User)
            .FirstOrDefaultAsync(rr => rr.Id == riderequestId);
    }
    public async Task<Dictionary<int, int>> GetAllByGivenParameter(string? type, int? year, int? month)
    {
        Dictionary<int, int> rideRequests = new();
        if (type == "monthly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .Where(r => r.DateCreated.Year == year)
                 .GroupBy(item => item.DateCreated.Month)
                 .ToDictionaryAsync(g => g.Key, g => g.Count());

            for(int i = 1; i < 13; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
        if (type == "weekly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                .Where(r => r.DateCreated.Year == year)
                .Where(r => r.DateCreated.Month == month)
                .GroupBy(r => r.DateCreated.Day / 7 + 1)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
          for(int i = 1; i < 6; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
         if (type == "yearly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .GroupBy(item => item.DateCreated.Year)
                 .ToDictionaryAsync(g => g.Key, g => g.Count());

            for(int i = 2023; i < DateTime.UtcNow.Year; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }

      
        return rideRequests;
    }

    public async Task<Dictionary<int, int>> GetRideRequestStatistics(string? type, int? year, int? month, Status status)
    {
        Dictionary<int, int> rideRequests = new();
        if (type == "monthly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .Where(r => r.Status == status)
                 .Where(r => r.DateCreated.Year == year)
                 .GroupBy(item => item.DateCreated.Month)
                 .ToDictionaryAsync(g => g.Key, g => g.Count());


                   for(int i = 1; i < 13; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
        if (type == "weekly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                .Where(r => r.Status == status)
                .Where(r => r.DateCreated.Year == year)
                .Where(r => r.DateCreated.Month == month)
                .GroupBy(r => r.DateCreated.Day / 7 + 1)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            for(int i = 1; i < 6; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
        if (type == "yearly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .Where(r => r.Status == status)
                 .GroupBy(item => item.DateCreated.Year)
          
               .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

         for(int i = 2023; i < DateTime.UtcNow.Year; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        return rideRequests;
    }


    public async Task<PaginatedResponse<RideRequest>> SearchByGivenParameter(int PageNumber, int PageSize, Status? status, double? fare, string? name, string? phoneNumber)
    {

        var response = new PaginatedResponse<RideRequest>();

        var rideRequestQuery = _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Where(item => (item.Status == (status ?? item.Status)))
            .Where(item => (item.User.PhoneNumber == (phoneNumber ?? item.User.PhoneNumber)))
            .Where(item => (item.CurrentFare <= (fare ?? item.CurrentFare)))
            .Where(item => (item.User.FullName == (name ?? item.User.FullName)));
        var count = await rideRequestQuery.CountAsync();
        var rideRequests = await rideRequestQuery
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .Include(rr => rr.User)
            .Skip((PageNumber - 1) * PageSize)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        response.Value = rideRequests;
        response.Count = count;
        return response;

    }

    public async Task<Dictionary<string, Dictionary<int, int>>> GetAllByGivenStatus(string? type, int? year, int? month)
    {
        return new Dictionary<string, Dictionary<int, int>>(){
            {"failed", await GetRideRequestStatistics(type,year, month, Status.CANCELLED)},
            {"completed", await GetRideRequestStatistics(type,year, month, Status.COMPLETED)}
        };
    }



    public async Task<PaginatedResponse<RideRequest>> GetAllUserRequests(int PageNumber, int PageSize, string UserId)
    {

        var response = new PaginatedResponse<RideRequest>();
        var query = _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Where(r => r.UserId == UserId);
           
        var count = await query.CountAsync();
        var rideRequests = await query.Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .Include(riderequest => riderequest.Origin)
            .Include(riderequest => riderequest.Destination)
            .Include(riderequest => riderequest.User)
            .ToListAsync();

        response.Count = count;
        response.Value = rideRequests;
        return response;

    }
    

    public async Task<Dictionary<string, int>> GetStatusStatistics()
    {
          return new Dictionary<string, int>(){
            {"WAITING", await GetStatusNumber(Status.WAITING)},
            {"ONROUTE", await GetStatusNumber(Status.ONROUTE)},
            {"COMPLETED", await GetStatusNumber(Status.COMPLETED)},
            {"CANCELLED", await GetStatusNumber(Status.CANCELLED)} 
        };
    }

    public async Task<int> GetStatusNumber (Status status) {
        var response = new PaginatedResponse<RideRequest>();
        var query = _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Where(r => r.Status == status);
           
        var count = await query.CountAsync();
        return count;
    }


}
