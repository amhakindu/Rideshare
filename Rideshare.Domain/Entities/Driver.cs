using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Domain.Entities
{
    public class Driver : BaseEntity
    {
        public User User { get; set; } = new User();
        public int Rate { get; set; }
        public double Experience { get; set; }
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
    }

}

