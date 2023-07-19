using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;

namespace Rideshare.Application.Features.Vehicles.Queries
{
     
    public class GetNumberOfVehicleQuery: TimeseriesQuery, IRequest<BaseResponse<Dictionary<int, int>>>
    {
    }
}
