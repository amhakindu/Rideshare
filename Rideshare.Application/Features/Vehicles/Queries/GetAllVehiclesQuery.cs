using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.Vehicles;

namespace Rideshare.Application.Features.Vehicles.Queries;
public class GetAllVehiclesQuery : PaginatedQuery, IRequest<PaginatedResponse<VehicleDto>>
{
}
