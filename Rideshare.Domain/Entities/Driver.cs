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
        public double Rate { get; set; } = 0;
        public double Experience { get; set; }
        public string Address { get; set; } 
        public string LicenseNumber { get; set; } 
        public string License { get; set; } 
    }

}

