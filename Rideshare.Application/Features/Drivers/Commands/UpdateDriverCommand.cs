using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Commands
{
    public class UpdateDriverCommand : IRequest<BaseResponse<Unit>>
    {
        public UpdateDriverDto UpdateDriverDto { get; set; }
        public string UserId {get; set;}
    }
}
