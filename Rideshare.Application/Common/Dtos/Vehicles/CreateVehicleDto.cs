using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Vehicles;
public class CreateVehicleDto
{
    public string PlateNumber { get; set; }
    public int NumberOfSeats { get; set; }
    public string Model { get; set; }
    public IFormFile Libre { get; set; }
    public int DriverId { get; set; }
}
