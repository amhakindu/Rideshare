// using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class RideOffer: BaseEntity
{ 
	public Driver Driver { get; set; }
	public int DriverId { get; set; }
	public Vehicle Vehicle { get; set; }    
	public int VehicleId { get; set; }
	public GeographicalLocation CurrentLocation { get; set; }
	public int CurrentLocationId { get; set; }
	public GeographicalLocation Destination { get; set; }
	public int DestinationId { get; set; }
	public Status Status { get; set; } = Status.WAITING;
	public int AvailableSeats { get; set; }
	public double EstimatedFare { get; set; }
	public TimeSpan EstimatedDuration { get; set; } 
	public ICollection<RideRequest> Matches = new List<RideRequest>();
}
