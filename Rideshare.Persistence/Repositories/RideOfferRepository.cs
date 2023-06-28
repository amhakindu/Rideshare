using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Persistence;
using static Rideshare.Application.Common.Constants.Utils;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Common;
using System.Globalization;

namespace Rideshare.Persistence.Repositories;

public class RideOfferRepository : GenericRepository<RideOffer>, IRideOfferRepository
{
    private const double RADIUS_IN_METERS = 150.0;
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
            .Include(rideoffer => rideoffer.Matches).FirstOrDefaultAsync();

        foreach (RideRequest riderequest in rideOffer.Matches){
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
            .FirstOrDefaultAsync();
    }
    public async Task<IReadOnlyList<RideOffer>> GetAll(int pageNumber, int pageSize)
    {
        return await _dbContext.Set<RideOffer>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(ro => ro.Driver)
            .Include(ro => ro.CurrentLocation)
            .Include(ro => ro.Destination)
            .ToListAsync();
    }
    
    public async Task<Dictionary<string, object>> GetAllPaginated(int pageNumber=1, int pageSize=10)
    {
        var query = _dbContext.Set<RideOffer>()
            .AsNoTracking();

        var count = await query.CountAsync();
        var rideoffers = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(ro => ro.Driver)
            .Include(ro => ro.CurrentLocation)
            .Include(ro => ro.Destination)
            .ToListAsync();
        return new Dictionary<string, object>(){
            {"count", count},
            {"rideoffers", rideoffers}
        };
    }
    public async Task<int> UpdateCurrentLocation(RideOffer rideoffer, GeographicalLocation location){
        var locations = await _dbContext.Locations.ToListAsync();
        var temp1 = locations.OrderBy(g => HaversineDistance(g.Coordinate, location.Coordinate))
            .FirstOrDefault();
        
        if (temp1 != null && HaversineDistance(temp1.Coordinate, location.Coordinate) <= RADIUS_IN_METERS)
            location = null;
        if(location == null)
            rideoffer.CurrentLocation = temp1;
        else
            rideoffer.CurrentLocation = location;
        return await Update(rideoffer);
    }
    public async Task<int> Add(RideOffer entity)
    {
        var locations = await _dbContext.Locations.ToListAsync();
        var temp1 = locations.OrderBy(g => HaversineDistance(g.Coordinate, entity.CurrentLocation.Coordinate))
            .FirstOrDefault();
        var temp2 = locations.OrderBy(g => HaversineDistance(g.Coordinate, entity.Destination.Coordinate))
            .FirstOrDefault();
        
        if (temp1 != null && HaversineDistance(temp1.Coordinate,entity.CurrentLocation.Coordinate) <= RADIUS_IN_METERS)
            entity.CurrentLocation = null;
        if (temp2 != null && HaversineDistance(temp2.Coordinate, entity.Destination.Coordinate) <= RADIUS_IN_METERS)
            entity.Destination = null;

        await _dbContext.AddAsync(entity);
        if(temp1 != null)
            entity.CurrentLocation = temp1;
        if(temp2 != null)
            entity.Destination = temp2;
        int id = await _dbContext.SaveChangesAsync();
        return await Update(entity);
    }

    public async Task<Dictionary<string, object>> GetRideOffersOfDriver(int DriverID, int PageNumber=1, int PageSize=10)
    {
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

        return new Dictionary<string, object>(){
            {"count", count},
            {"rideoffers", rideoffers}
        };
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
    public async Task<Dictionary<string, object>> SearchAndFilter(double MinCost, double MaxCost, string? driverName, string? driverPhoneNumber, Status? status, int PageNumber=1, int PageSize=10){
        var query = _dbContext.Set<RideOffer>()
            .Where(rideoffer => rideoffer.Driver.User.FullName == (driverName ?? rideoffer.Driver.User.FullName))
            .Where(rideoffer => rideoffer.Driver.User.PhoneNumber == (driverPhoneNumber ?? rideoffer.Driver.User.PhoneNumber))
            .Where(rideoffer => rideoffer.EstimatedFare >= MinCost)
            .Where(rideoffer => rideoffer.EstimatedFare <= MaxCost)
            .Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status));

        Int64 count = await query.CountAsync();
        var rideoffers = await query.Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .Include(ro => ro.Driver)
            .Include(ro => ro.CurrentLocation)
            .Include(ro => ro.Destination)
            .ToListAsync();
        return new Dictionary<string, object>(){
            {"count", count},
            {"rideoffers", rideoffers}
        };
    }

    public async Task<Dictionary<int, int>> GetRideOfferStatistics(int? year, int? month, Status? status){
        DbSet<RideOffer> rideOffers = _dbContext.Set<RideOffer>();
        if(month != null && year != null){
            // Weekly
            return await rideOffers.Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status))
                .Where(rideoffer => rideoffer.DateCreated.Year == year)
                .Where(rideoffer => rideoffer.DateCreated.Month == month)
                .GroupBy(rideoffer => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(rideoffer.DateCreated, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                .ToDictionaryAsync(group => group.Key, group => group.Count());

        }else if(month == null && year == null){
            // Yearly
            return await rideOffers
                .Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status))
                .GroupBy(rideoffer => rideoffer.DateCreated.Year)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }else{   
            // Monthly
            return await rideOffers.Where(rideoffer => rideoffer.Status == (status ?? rideoffer.Status))
                .Where(rideoffer => rideoffer.DateCreated.Year == year)
                .GroupBy(rideoffer => rideoffer.DateCreated.Month)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
    public async Task<Dictionary<string, Dictionary<int, int>>> GetRideOfferStatisticsWithStatus(int? year, int? month){
        return new Dictionary<string, Dictionary<int, int>>(){
            {"failed", await GetRideOfferStatistics(year, month, Status.CANCELLED)},
            {"completed", await GetRideOfferStatistics(year, month, Status.COMPLETED)}
        }; 
    }

    public Task<List<GeographicalLocation>> GetPopularDestinationsOfDriver(int driverId, int limit)
    {
        return _dbContext.Set<RideOffer>()
            .Where(ride => ride.Driver.Id == driverId)
            .GroupBy(ride => ride.Destination)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .Take(limit)
            .ToListAsync();
    }
}

    


