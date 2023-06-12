using Rideshare.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Domain.Entities;
public class Vehicle : BaseEntity
{
    public string PlateNumber { get; set; }
    public int NumberOfSeats { get; set; }
    public string Model { get; set; }
    public string Libre { get; set; }
    public string UserId { get; set; }
}
