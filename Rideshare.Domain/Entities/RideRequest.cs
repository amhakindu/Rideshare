using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;
using Rideshare.Domain.Models;

namespace Rideshare.Domain.Entities;

public class RideRequest : BaseEntity
{
    public Point Origin { get; set; }
    public Point Destination { get; set; }
    public double CurrentFare { get; set; }
    public Status Status { get; set; }
    public int NumberOfSeats { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}