using MediatR;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Queries
{
     
    public class GetNumberOfVehicleQuery: IRequest<BaseResponse<Dictionary<int, int>>>
    {
        public string option { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
