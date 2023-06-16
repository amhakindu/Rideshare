using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class RideOfferListDto
{
    public int Id { get; set; }
    public string DriverID { get; set; }
    public int VehicleID { get; set; }   
    public LocationDto CurrentLocation { get; set; }
    public LocationDto Destination { get; set; }
    public Status Status { get; set; }
    public int AvailableSeats { get; set; }
    public double EstimatedCost { get; set; }
    public TimeSpan EstimatedDuration { get; set; }
}
