
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public interface IRideRequestDto 
{
    public LocationDto Origin { get; set; }
    public LocationDto Destination { get; set; }
    public double CurrentFare { get; set; }
    public Status Status { get; set; }
}
