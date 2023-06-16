
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class UpdateRideRequestDto : IRideRequestDto
{
    public int Id { get; set; }
    public LocationDto Origin { get; set; }
    public LocationDto Destination { get; set; }
    public double CurrentFare { get; set; }
    public Status Status { get; set; }
}
