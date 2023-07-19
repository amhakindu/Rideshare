using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Features.Drivers.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/drivers")]
[Authorize]
public class DriverController : BaseApiController
{
    public DriverController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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

    [HttpGet("of-user/{userId}")]
    [Authorize(Roles ="Driver,Admin")]
    public async Task<IActionResult> GetUser(string userId)
    {
        var result = await _mediator.Send(new GetDriverByUserIdQuery { Id = userId });

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

    [HttpPut]
    [Authorize(Roles = "Driver")]
    public async Task<IActionResult> Update([FromBody] UpdateDriverDto updateDriverDto)
    {
        var result = await _mediator.Send(new UpdateDriverCommand { UpdateDriverDto = updateDriverDto , UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpPut("approve/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve([FromBody] ApproveDriverDto approveDriverDto)
    {
        var result = await _mediator.Send(new ApproveDriverCommand { ApproveDriverDto = approveDriverDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}