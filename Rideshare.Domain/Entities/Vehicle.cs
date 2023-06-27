using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;
public class Vehicle : BaseEntity
{
    public string PlateNumber { get; set; }
    public int NumberOfSeats { get; set; }
    public string Model { get; set; }
    public string Libre { get; set; }
    public int DriverId { get; set; }
    public Driver Driver { get; set; }
}
