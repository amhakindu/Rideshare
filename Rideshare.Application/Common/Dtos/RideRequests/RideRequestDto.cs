
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class RideRequestDto : IRideRequestDto
{
    public int Id { get; set; }
    public LocationDto Origin { get; set; }
    public LocationDto Destination { get; set; }    
    public string originAddress { get; set; }
    public string destinationAddress { get; set; }
    public double CurrentFare { get; set; }
    public Status Status { get; set; } 
    public int NumberOfSeats { get; set; }
    public UserDto User { get; set; }
    public bool Accepted { get; set; }
}
