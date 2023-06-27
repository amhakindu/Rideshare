using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;
using Rideshare.Domain.Models;

namespace Rideshare.Domain.Entities;

public class RideRequest : BaseEntity
{
    public GeographicalLocation Origin { get; set; }
    public GeographicalLocation Destination { get; set; }
    public double CurrentFare { get; set; } = 0;
    public int NumberOfSeats { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public Status Status { get; set; } = Status.WAITING;
    public RideOffer? MatchedRide { get; set; } = null;
    public bool Accepted { get; set; } = false;    
}