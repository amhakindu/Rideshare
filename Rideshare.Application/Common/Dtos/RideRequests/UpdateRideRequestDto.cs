
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class UpdateRideRequestDto
{
    public int Id { get; set; }
    public LocationDto? Origin { get; set; }
    public LocationDto? Destination { get; set; }
    public int NumberOfSeats { get; set; }
    public string? UserId { get; set; }
}
