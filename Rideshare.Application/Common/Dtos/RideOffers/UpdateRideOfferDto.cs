
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class UpdateRideOfferDto
{
    public int Id { get; set; }
    public int? VehicleID { get; set; }   
    public LocationDto? CurrentLocation { get; set; }
    public LocationDto? Destination { get; set; }
    public Status? Status { get; set; }
}
