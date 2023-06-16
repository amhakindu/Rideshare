using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Queries;
public class GetVehicleQuery : IRequest<BaseResponse<VehicleDto>>
{
    public int VehicleId { get; set; }
}
