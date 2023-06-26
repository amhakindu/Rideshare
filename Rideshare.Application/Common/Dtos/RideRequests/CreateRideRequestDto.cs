 
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class CreateRideRequestDto
{
    public LocationDto Origin { get; set; }
    public LocationDto Destination { get; set; }
    public int NumberOfSeats { get; set; }
    public string? UserId { get; set; }

}
