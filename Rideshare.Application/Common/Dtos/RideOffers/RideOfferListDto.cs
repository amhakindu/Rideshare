using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class RideOfferListDto
{
    public int Id { get; set; }
    public DriverDetailDto Driver { get; set; }   
    public string OriginAddress { get; set; }
    public string DestinationAddress { get; set; }
    public string Status { get; set; }
    public int AvailableSeats { get; set; }
}
