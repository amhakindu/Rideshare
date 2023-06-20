using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Vehicles;
public class VehicleDto
{
    public int Id { get; set; }
    public string PlateNumber { get; set; }
    public int NumberOfSeats { get; set; }
    public string Model { get; set; }
    public string Libre { get; set; }
    public int DriverId { get; set; }
}
