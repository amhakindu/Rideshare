using Rideshare.Domain.Common;
using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Domain.Entities
{
    public class Feedback: BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Double Rating { get; set; }
    }
}
