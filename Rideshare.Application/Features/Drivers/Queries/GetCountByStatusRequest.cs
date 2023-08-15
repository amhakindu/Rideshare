using System;
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetCountByStatusRequest : IRequest<BaseResponse<StatusDto>>
    {
        
    }
}