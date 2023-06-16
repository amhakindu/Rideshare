using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class RideRequest : BaseEntity
{
    public Point Origin { get; set; }
    public Point Destination { get; set; }
    public double CurrentFare { get; set; }
    public Status Status { get; set; }
}