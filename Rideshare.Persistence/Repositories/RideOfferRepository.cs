using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System.Collections.Generic;

namespace Rideshare.Persistence.Repositories;

public class RideOfferRepository : GenericRepository<RideOffer>, IRideOfferRepository
{
    private readonly RideshareDbContext _dbContext;
    public RideOfferRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<RideOffer>> GetRideOffersOfDriver(string DriverID){
        return (IReadOnlyList<RideOffer>) _dbContext.RideOffers
            .Where(rideOffer => rideOffer.DriverID == DriverID).ToList();
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
            .GroupBy(ro => ro.DriverID)
            .Select(g => new DriverStatsDto
            {
                DriverID = g.Key,
                TotalOffers = g.Count(),
                Earnings = g.Sum(ro => ro.EstimatedFare)
            })
            .OrderByDescending(dto => dto.TotalOffers)
            .Take(5)
            .ToListAsync();

        return result;
    }
}

    


