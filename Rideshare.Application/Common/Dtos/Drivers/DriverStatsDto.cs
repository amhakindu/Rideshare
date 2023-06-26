using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Drivers
{
    public class DriverStatsDto
    {
        public string DriverID { get; set; }
        public int TotalOffers { get; set; }
        public double Earnings { get; set; }
    }
}
