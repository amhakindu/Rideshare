using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Responses;
using System.Net;

namespace Rideshare.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DriverController : BaseApiController
    {
        private readonly IUserAccessor _userAccessor;
        public DriverController(IMediator mediator, IUserAccessor userAccessor) : base(mediator)
        {
            _userAccessor = userAccessor;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetDriversListRequest { PageNumber=pageNumber, PageSize = pageSize });
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);


        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetDriverRequest { Id = id });

            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpPost]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> Post([FromForm] CreateDriverDto createDriverDto)
        {
            var result = await _mediator.Send(new CreateDriverCommand { CreateDriverDto = createDriverDto, UserId = _userAccessor.GetUserId() });

            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse(status, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDriverCommand { Id = id , UserId = _userAccessor.GetUserId()});

            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> Update([FromBody] UpdateDriverDto updateDriverDto)
        {
            var result = await _mediator.Send(new UpdateDriverCommand { UpdateDriverDto = updateDriverDto , UserId = _userAccessor.GetUserId()});

            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }


        [HttpPut("Approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve([FromBody] ApproveDriverDto approveDriverDto)
        {
            var result = await _mediator.Send(new ApproveDriverCommand { ApproveDriverDto = approveDriverDto });

            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpGet("statistics")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Get(bool weekly, bool monthly, bool yearly)
        {
            var result = await _mediator.Send(new GetDriversStatisticsRequest { Weekly = weekly, Monthly = monthly, Yearly = yearly });
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);


        }



    }
}
