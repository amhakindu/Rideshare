using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class RideOfferDto
{
    public int Id { get; set; }
    public DriverDetailDto Driver { get; set; }
    public VehicleDto Vehicle;    
    public LocationDto CurrentLocation { get; set; }
    public LocationDto Destination { get; set; }  
    public string OriginAddress { get; set; }
    public string DestinationAddress { get; set; }
    public string Status { get; set; }
    public int AvailableSeats { get; set; }
    public double EstimatedFare { get; set; }
    public TimeSpan EstimatedDuration { get; set; }  
}
