using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.Rates;

namespace Rideshare.Application.Features.Rates.Queries;

public class GetRateListQuery : PaginatedQuery, IRequest<PaginatedResponse<RateDto>>
{
    public string UserId { get; set; }
}

