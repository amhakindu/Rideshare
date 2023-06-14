using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Drivers
{
    public class DriverDetailDto 
    {
        public int Id { get; set; }
        public User User { get; set; } = new User();
        public int Rate { get; set; }
        public double Experience { get; set; }
        public string Address { get; set; } 
        public string LicenseNumber { get; set; }
        public string License { get; set; }
        public bool Verified { get; set; }
    }
}
