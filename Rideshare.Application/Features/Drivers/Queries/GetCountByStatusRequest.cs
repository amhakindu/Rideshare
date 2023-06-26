using System;
using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetCountByStatusRequest : IRequest<BaseResponse<List<int>>>
    {
        
    }
}