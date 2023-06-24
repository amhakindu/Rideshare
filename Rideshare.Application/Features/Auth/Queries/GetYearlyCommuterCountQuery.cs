using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Queries;

public class GetYearlyCommuterCountQuery : IRequest<BaseResponse<YearlyCommuterCountDto>>
{
}
