namespace Rideshare.Application.Common.Dtos.RideOffers;

public class CreateRideOfferDto
{
    public string DriverID { get; set; }
    public int VehicleID { get; set; }   
    public LocationDto CurrentLocation { get; set; }
    public LocationDto Destination { get; set; }
}
