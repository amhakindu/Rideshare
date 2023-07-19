using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetDriversListRequest : PaginatedQuery, IRequest<PaginatedResponse<DriverDetailDto>>
    {
    }
}
