using MediatR;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Commands;
public class DeleteVehicleCommand : IRequest<BaseResponse<Unit>>
{
    public int VehicleId { get; set; }
}
