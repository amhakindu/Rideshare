using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.RideOffers.Queries
{
    public class GetTopDriversWithStatsRequest : IRequest<BaseResponse<List<DriverStatsDto>>>
    {
    }
}
