using Rideshare.Application.Common.Dtos.RideRequests;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class CommuterViewOfRideOfferDto
{
    public string DriverName { get; set; }
    public string DriverImageUrl { get; set; }
    public double AverageRate { get; set; }
    public string VehicleModel { get; set; }
    public int AvailableSeats { get; set; }    
    public string VehiclePlateNumber { get; set; }
    public string DriverPhoneNumber { get; set; }
    public LocationDto CurrentLocation { get; set; }
    public List<RideRequestDto> Matches { get; set; }
}
