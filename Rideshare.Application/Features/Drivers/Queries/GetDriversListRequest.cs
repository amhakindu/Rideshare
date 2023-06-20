using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetDriversListRequest : IRequest<BaseResponse<List<DriverDetailDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
