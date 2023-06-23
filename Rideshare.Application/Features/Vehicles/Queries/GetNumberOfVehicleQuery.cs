using MediatR;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Queries
{
    public class GetNumberOfVehicleQuery: IRequest<BaseResponse<int>>
    {
        public int Days { get; set; }
    }
}
