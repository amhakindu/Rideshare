
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public interface IRideRequestDto 
{
    public string originAddress { get; set; }
    public string destinationAddress { get; set; }
    public int NumberOfSeats { get; set; }
    // public string UserId { get; set; }
    
}
