using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Queries
{
    public class GetCommuterStatusQuery : IRequest<BaseResponse<CommuterStatusDto>>
    {   
        public int TimeIntervalMinutes { get; set; }
    }
}