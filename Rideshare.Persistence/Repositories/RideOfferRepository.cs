using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Persistence.Repositories;

public class RideOfferRepository : GenericRepository<RideOffer>, IRideOfferRepository
{
	private const double RADIUS_IN_KM = 0.015;
	private readonly RideshareDbContext _dbContext;
	public RideOfferRepository(RideshareDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
	public async Task<RideOffer> AcceptRideRequest(int riderequestId){
		var rideRequest = _dbContext.Set<RideRequest>()
			.Include(rideoffer => rideoffer.MatchedRide)
			.FirstOrDefault(rideoffer => rideoffer.Id == riderequestId);

		rideRequest.Accepted = true;
		var matchedRideOffer = rideRequest.MatchedRide;
		matchedRideOffer.AvailableSeats -= rideRequest.NumberOfSeats;

		_dbContext.Update(rideRequest);
		_dbContext.Update(matchedRideOffer);

		await _dbContext.SaveChangesAsync();

		return matchedRideOffer;
	}
	public async Task<int> CancelRideOffer(int rideOfferId){
		var rideOffer = await _dbContext.Set<RideOffer>()
			.Where(rideoffer => rideoffer.Id == rideOfferId)
			.Include(rideoffer => rideoffer.Matches).FirstOrDefaultAsync();

		ICollection<RideRequest> rideoffers = rideOffer.Matches.ToList();
		foreach (RideRequest riderequest in rideoffers){
			riderequest.MatchedRide = null;
			riderequest.Accepted = false;
			_dbContext.Update(riderequest);
			await _dbContext.SaveChangesAsync();
		}
		rideOffer.Status = Status.CANCELLED;
		_dbContext.Update(rideOffer);

		return await _dbContext.SaveChangesAsync(); 
	}
	public async Task<RideOffer?> Get(int id)
	{
		return _dbContext.Set<RideOffer>()
			.AsNoTracking()
			.Include(ro => ro.Driver)
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.FirstOrDefault(ro => ro.Id == id);
	}
	public async Task<IReadOnlyList<RideOffer>> GetActiveRideOffers(){
		return await _dbContext.Set<RideOffer>()
			.AsNoTracking()
			.Include(ro => ro.Driver)
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.Where(rideoffer => rideoffer.Status == Status.ONROUTE || rideoffer.Status == Status.WAITING)
			.ToListAsync();
	}
	public async Task<RideOffer?> GetActiveRideOfferOfDriver(int DriverId){
		return await _dbContext.Set<RideOffer>()
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.Where(ro => ro.Driver.Id == DriverId)
			.Where(ro => (ro.Status == Status.WAITING) || (ro.Status == Status.ONROUTE))
			.FirstOrDefaultAsync();
	}
	public async Task<PaginatedResponse<RideOffer>> GetAll(int pageNumber, int pageSize)
	{
		var response = new PaginatedResponse<RideOffer>();
		
		var rideOffers = await _dbContext.Set<RideOffer>()
			.AsNoTracking()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.Include(ro => ro.Driver)
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.ToListAsync();
		response.Count = await _dbContext.RideOffers.CountAsync();
		response.Value = rideOffers;
		return response;
	}
	
	public async Task<PaginatedResponse<RideOffer>> GetAllPaginated(int pageNumber=1, int pageSize=10)
	{
		var response = new PaginatedResponse<RideOffer>();
		var query = _dbContext.Set<RideOffer>()
			.AsNoTracking();

		var rideoffers = await query
			.OrderBy(rideoffer => rideoffer.DateCreated)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.Include(ro => ro.Driver)
				.ThenInclude(driver => driver.User)
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.ToListAsync();

		response.Count = await query.CountAsync();
		response.Value = rideoffers;
		return response;
		
	}
	public async Task<int> UpdateCurrentLocation(RideOffer rideoffer, GeographicalLocation location){
		var temp1 = await _dbContext.Locations.OrderBy(g => RideshareDbContext.haversine_distance(g.Coordinate.X, g.Coordinate.Y, location.Coordinate.X, location.Coordinate.Y))
			.Where(location => RideshareDbContext.haversine_distance(location.Coordinate.X, location.Coordinate.Y, location.Coordinate.X, location.Coordinate.Y) <= RADIUS_IN_KM)
			.FirstOrDefaultAsync();
		
		rideoffer.CurrentLocation = temp1 ?? location;

		return await Update(rideoffer);
	}
	public async Task<int> Add(RideOffer entity)
	{
		Point current= entity.CurrentLocation.Coordinate;
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
			throw new ValidationException($"Failed to Create Rideoffer! Origin and Detination must be farther than {RADIUS_IN_KM} km");

		entity.CurrentLocation = temp1 ?? entity.CurrentLocation;
		entity.Destination = temp2 ?? entity.Destination;

		await _dbContext.AddAsync(entity);

		return await _dbContext.SaveChangesAsync();
	}

	public async Task<PaginatedResponse<RideOffer>> GetRideOffersOfDriver(int DriverID, int PageNumber=1, int PageSize=10)
	{
		var response = new PaginatedResponse<RideOffer>();
		var query = _dbContext.Set<RideOffer>()
			.AsNoTracking()
			.Where(rideOffer => rideOffer.Driver.Id == DriverID);

		var count = await query.CountAsync();
		var rideoffers = await query
			.Skip((PageNumber - 1) * PageSize)
			.Take(PageSize)
			.Include(ro => ro.Driver)
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.ToListAsync();

		response.Count = await query.CountAsync();
		response.Value = rideoffers;

		return response;	
	}

	public async Task<IReadOnlyList<ModelAndCountDto>> NoTopModelOffers()
	{
		var result = await _dbContext.RideOffers
			.GroupBy(ro => ro.Vehicle.Model)
			.OrderByDescending(g => g.Count())
			.Take(10)
			.Select(g => new ModelAndCountDto { Model = g.Key, Count = g.Count() })
			.ToListAsync();

		return result;
	}

	public async Task<List<DriverStatsDto>> GetTopDriversWithStats()
	{
		var result = await _dbContext.RideOffers
			.GroupBy(ro => ro.Driver.Id)
			.Select(g => new DriverStatsDto
			{
				DriverID = g.Key.ToString(),
				TotalOffers = g.Count(),
				Earnings = g.Sum(ro => ro.EstimatedFare)
			})
			.OrderByDescending(dto => dto.TotalOffers)
			.Take(5)
			.ToListAsync();

		return result;
	}
	public async Task<PaginatedResponse<RideOffer>> SearchAndFilter(double MinCost, double MaxCost, string? driverName, string? driverPhoneNumber, Status? status, int PageNumber=1, int PageSize=10){

		var response = new PaginatedResponse<RideOffer>();

		var query = _dbContext.Set<RideOffer>()
			.Where(rideoffer => rideoffer.Driver.User.FullName == (driverName ?? rideoffer.Driver.User.FullName))
			.Where(rideoffer => rideoffer.Driver.User.PhoneNumber == (driverPhoneNumber ?? rideoffer.Driver.User.PhoneNumber))
			.Where(rideoffer => rideoffer.EstimatedFare >= MinCost)
			.Where(rideoffer => rideoffer.EstimatedFare <= MaxCost)
			.Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status));

		int count = await query.CountAsync();
		var rideoffers = await query.Skip((PageNumber - 1) * PageSize)
			.Take(PageSize)
			.Include(ro => ro.Driver)
				.ThenInclude(driver => driver.User)
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.ToListAsync();
		
		response.Count = await query.CountAsync();
		response.Value = rideoffers;
		return response;
	}

	public async Task<Dictionary<int, int>> GetRideOfferStatistics(int? year, int? month, Status? status){
		DbSet<RideOffer> rideOffers = _dbContext.Set<RideOffer>();
		if(month != null && year != null){
			// Weekly
			var temp = await rideOffers.Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status))
				.Where(rideoffer => rideoffer.DateCreated.Year == year)
				.Where(rideoffer => rideoffer.DateCreated.Month == month)
				.GroupBy(rideoffer => rideoffer.DateCreated.Day / 7 + 1)
				.ToDictionaryAsync(group => group.Key, group => group.Count());
			for (int i = 1; i <= 5; i++)
			{
				if(!temp.ContainsKey(i))
					temp.Add(i, 0);
			}
			return temp;
		}else if(month == null && year == null){
			// Yearly
			var temp = await rideOffers
				.Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status))
				.GroupBy(rideoffer => rideoffer.DateCreated.Year)
				.ToDictionaryAsync(g => g.Key, g => g.Count());
			for (int i = 2023; i <= DateTime.UtcNow.Year; i++)
			{
				if(!temp.ContainsKey(i))
					temp.Add(i, 0);
			}
			return temp;
		}else{   
			// Monthly
			Dictionary<int, int> temp = await rideOffers.Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status))
				.Where(rideoffer => rideoffer.DateCreated.Year == year)
				.GroupBy(rideoffer => rideoffer.DateCreated.Month)
				.ToDictionaryAsync(g => g.Key, g => g.Count());
			for (int i = 1; i < 13; i++)
			{
				if(!temp.ContainsKey(i))
					temp.Add(i, 0);
			}
			return temp;
		}
	}
	public async Task<Dictionary<string, Dictionary<int, int>>> GetRideOfferStatisticsWithStatus(int? year, int? month){
		return new Dictionary<string, Dictionary<int, int>>(){
			{"failed", await GetRideOfferStatistics(year, month, Status.CANCELLED)},
			{"completed", await GetRideOfferStatistics(year, month, Status.COMPLETED)}
		}; 
	}

	public async Task<List<GeographicalLocation>> GetPopularDestinationsOfDriver(int driverId, int limit)
	{
		return await _dbContext.Set<RideOffer>()
			.Where(ride => ride.Driver.Id == driverId)
			.GroupBy(ride => ride.Destination)
			.OrderByDescending(group => group.Count())
			.Select(group => group.Key)
			.Take(limit)
			.ToListAsync();
	}

	public async Task<Dictionary<string, int>> GetRideOfferCountForEachStatus()
	{
		return await _dbContext.Set<RideOffer>()
			.GroupBy(ride => ride.Status)
			.ToDictionaryAsync(group => Enum.GetName(typeof(Status), group.Key) ?? "", group => group.Count());
	}

	public async Task<RideOffer?> GetRideOfferWithDetail(int Id)
	{
		return await _dbContext.Set<RideOffer>()
			.AsNoTracking()
			.Include(ro => ro.Driver)
				.ThenInclude(driver => driver.User)
			.Include(ro => ro.Vehicle)
			.Include(ro => ro.CurrentLocation)
			.Include(ro => ro.Destination)
			.Include(ro => ro.Matches)
				.ThenInclude(rr => rr.Origin)
			.Include(ro => ro.Matches)
				.ThenInclude(rr => rr.Destination)
			.Include(ro => ro.Matches)
				.ThenInclude(rr => rr.User)
			.FirstOrDefaultAsync(ro => ro.Driver.Id == Id);
	}
}

	


