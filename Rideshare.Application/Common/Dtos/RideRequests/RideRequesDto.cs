
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class RideRequestDto : IRideRequestDto
{
    public int Id { get; set; }
    public string originAddress { get; set; }
    public string destinationAddress { get; set; }
    public double CurrentFare { get; set; }
    public Status Status { get; set; } 
    public int NumberOfSeats { get; set; }
    public string UserId { get; set; }
}
