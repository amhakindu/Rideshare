using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Userss;

namespace Rideshare.WebApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]

public class RideRequestController : BaseApiController
{
    private readonly IUserAccessor _userAccessor;
    public RideRequestController(IMediator mediator,IUserAccessor userAccessor) : base(mediator)
    {
        _userAccessor = userAccessor;
    }

   
    [Authorize(Roles = "Commuter")]
    [HttpGet("{id}")]

    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideRequestQuery { Id = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [Authorize(Roles = "Commuter")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetRideRequestListQuery{UserId = _userAccessor.GetUsername()});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [Authorize(Roles = "Commuter")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateRideRequestDto rideRequestDto)
    {
        var result = await _mediator.Send(new CreateRideRequestCommand { RideRequestDto = rideRequestDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [Authorize(Roles = "Commuter")]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateRideRequestDto rideRequestDto)
    {
        var result = await _mediator.Send(new UpdateRideRequestCommand { RideRequestDto = rideRequestDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [Authorize(Roles = "Commuter")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideRequestCommand { Id = id });

        var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}
