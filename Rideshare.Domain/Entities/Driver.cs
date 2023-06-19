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
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public List<double>  Rate { get; set; } = new List<double> (){0.0, 0.0, 0.0};
        public double Experience { get; set; }
        public bool Verified { get; set; } = false;
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
    }

}

