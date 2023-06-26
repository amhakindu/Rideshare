using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Commands;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Userss;

namespace Rideshare.WebApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]

public class RideRequestController : BaseApiController
{
    public RideRequestController(IMediator mediator,IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

   
    [Authorize(Roles = "Commuter,Admin")]
    [HttpGet("{id}")]

    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideRequestQuery { Id = id,UserId = _userAccessor.GetUserId() });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetRideRequestListQuery{UserId = _userAccessor.GetUserId(), PageNumber=pageNumber, PageSize = pageSize});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpPost]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> Post([FromBody] CreateRideRequestDto rideRequestDto)
    {
        rideRequestDto.UserId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new CreateRideRequestCommand { RideRequestDto = rideRequestDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [Authorize(Roles = "Commuter")]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateRideRequestDto rideRequestDto)
    {
        var result = await _mediator.Send(new UpdateRideRequestCommand { RideRequestDto = rideRequestDto ,UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [Authorize(Roles = "Commuter")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideRequestCommand { Id = id ,UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}
