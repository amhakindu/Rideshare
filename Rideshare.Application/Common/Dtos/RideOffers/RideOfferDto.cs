using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class RideOfferDto
{
    public int Id { get; set; }
    public string DriverID { get; set; }
    public string VehicleID { get; set; }   
    public LocationDto CurrentLocation { get; set; }
    public LocationDto Destination { get; set; }
    public Status Status { get; set; }
    public int AvailableSeats { get; set; }
    public double EstimatedFare { get; set; }
    public TimeSpan EstimatedDuration { get; set; }  
}
