using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers;

public class RideOffersListFilterDto
{
    public string? DriverName { get; set; }
    public string? PhoneNumber { get; set; }
    public double MinCost { get; set; } = 0;
    public double MaxCost { get; set; } = double.MaxValue;    
    public string? Status { get; set; }      
}
