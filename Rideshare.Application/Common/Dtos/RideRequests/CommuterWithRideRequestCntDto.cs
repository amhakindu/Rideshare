using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.RideRequests
{
    public class CommuterWithRideRequestCntDto
    {
        public ApplicationUser Commuter { get; set; }
        public int RideRequestCount { get; set; }
    }
}
