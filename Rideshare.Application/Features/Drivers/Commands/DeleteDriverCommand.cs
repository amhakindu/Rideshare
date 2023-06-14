using MediatR;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Commands
{
    public class DeleteDriverCommand : IRequest<BaseResponse<Unit>>
    {
        public int Id { get; set; }
    }
}
