using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.RideRequests.Queries
{
    public class GetTop5CommuterQuery: IRequest<BaseResponse<List<CommuterWithRideRequestCntDto>>>
    {

    }
}
