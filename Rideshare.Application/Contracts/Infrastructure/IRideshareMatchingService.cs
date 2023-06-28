using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Infrastructure;

public interface IRideshareMatchingService
{
    public Task<RideOffer?> MatchWithRideoffer(RideRequest rideRequest);
}
