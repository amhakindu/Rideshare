using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Vehicles;
public class UpdateVehicleDto
{
    public int Id { get; set; }
    public string? PlateNumber { get; set; }
    public string? Libre { get; set; }
    public string? UserId { get; set; }
}
