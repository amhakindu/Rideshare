using Rideshare.Application.Common.Dtos.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class CreateRideOfferDto
{
    public int VehicleID { get; set; }   
    public LocationDto CurrentLocation { get; set; }
    public LocationDto Destination { get; set; }
}
