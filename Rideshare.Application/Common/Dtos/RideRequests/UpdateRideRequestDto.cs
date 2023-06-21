
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class UpdateRideRequestDto : IRideRequestDto
{
    public int Id { get; set; }
    public LocationDto Origin { get; set; }
    public LocationDto Destination { get; set; }
    public double CurrentFare { get; set; }
    public Status Status { get; set; }
    public int NumberOfSeats { get; set; }
    public string UserId { get; set; }
}
