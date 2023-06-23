using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Queries
{
    public class GetNumberOfCommutersQuery : IRequest<BaseResponse<CommuterStatusDto>>
    {
        public int TimeIntervalMinutes { get; set; }
    }
}