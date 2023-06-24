using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Drivers
{
    public class ApproveDriverDto  
    {
        public int Id { get; set; }
        public bool Verified { get; set; }

    }
}
