using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Queries;
public class GetAllVehiclesQuery : IRequest<BaseResponse<IList<VehicleDto>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
