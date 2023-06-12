using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Responses;
using System.Net;

namespace Rideshare.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : BaseApiController
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public DriverController(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator, unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetDriversListRequest());
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetDriverRequest { Id = id });

            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateDriverDto createDriverDto)
        {
            var result = await _mediator.Send(new CreateDriverCommand { CreateDriverDto = createDriverDto });

            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse(status, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDriverCommand { Id = id });

            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateDriverDto updateDriverDto)
        {
            var result = await _mediator.Send(new UpdateDriverCommand { UpdateDriverDto = updateDriverDto });

            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }
    }
}
