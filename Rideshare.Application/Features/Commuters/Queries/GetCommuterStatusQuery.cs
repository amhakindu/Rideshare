using MediatR;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Commuters.Queries
{
    public class GetCommuterStatusQuery : IRequest<BaseResponse<StatusDto>>
    {   
    }
}