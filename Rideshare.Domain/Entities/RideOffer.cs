// using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class RideOffer: BaseEntity
{
    public string DriverID { get; set; }
    public int VehicleID { get; set; }
    public Vehicle Vehicle { get; set; }
    public Point CurrentLocation { get; set; }
    public Point Destination { get; set; }
    public Status Status { get; set; } = Status.WAITING;
    public int AvailableSeats { get; set; }
    public double EstimatedFare { get; set; }
    public TimeSpan EstimatedDuration { get; set; }  
}
