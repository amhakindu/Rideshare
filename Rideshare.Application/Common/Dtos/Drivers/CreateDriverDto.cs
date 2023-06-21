using Microsoft.AspNetCore.Http;
using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Drivers
{
    public class CreateDriverDto
    {
        public string UserId {get; set;}
        public double Experience { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }
        public IFormFile License { get; set; }
    }
}
