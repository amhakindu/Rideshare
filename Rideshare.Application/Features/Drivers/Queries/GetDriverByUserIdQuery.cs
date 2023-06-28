using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetDriverByUserIdQuery : IRequest<BaseResponse<DriverDetailDto>>
    {
        public string Id { get; set; }
    }
}
