using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Commands;
public class CreateVehicleCommand : IRequest<BaseResponse<Nullable<int>>>
{
    public CreateVehicleDto VehicleDto { get; set; }
}
