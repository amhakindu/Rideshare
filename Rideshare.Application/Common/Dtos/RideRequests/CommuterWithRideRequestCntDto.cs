
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Common.Dtos.RideRequests
{
    public class CommuterWithRideRequestCntDto
    {
        public UserDtoForAdmin Commuter { get; set; }
        public int RideRequestCount { get; set; }
    }
}
