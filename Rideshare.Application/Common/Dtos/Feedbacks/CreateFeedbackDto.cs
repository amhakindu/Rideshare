using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Feedbacks
{
    public class CreateFeedbackDto: CommonDto
    {
        public string UserId { get; set; }
    }
}
