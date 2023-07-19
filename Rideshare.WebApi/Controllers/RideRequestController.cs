using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Features.RideRequests.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/riderequests")]

public class RideRequestController : BaseApiController
{
    public RideRequestController(IMediator mediator,IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Commuter")]
    [ProducesResponseType(typeof(BaseResponse<RideRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideRequestQuery { Id = id,UserId = _userAccessor.GetUserId() });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<RideRequestDto>>(status, result);
    }

    
    [HttpGet("filter")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<string, IReadOnlyList<RideRequestDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommuterRequests([FromQuery] RideRequestsListFilterDto RideRequestsListFilterDto,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
     var result = await _mediator.Send(new GetRideRequestListQuery{ RideRequestsListFilterDto = RideRequestsListFilterDto,PageNumber=pageNumber, PageSize = pageSize});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> GetRideRequestsOfLoggedInCommuter([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
     var result = await _mediator.Send(new GetRideRequestUserQuery {PageNumber=pageNumber, PageSize = pageSize,UserId = _userAccessor.GetUserId() });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
        
    [HttpGet("of-commuter/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRideRequestsOfUserWithId(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
     var result = await _mediator.Send(new GetRideRequestUserQuery {PageNumber=pageNumber, PageSize = pageSize, UserId= userId });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetRideRequestAllListQuery{PageNumber=pageNumber, PageSize = pageSize});

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

    
    [HttpPut]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> Put([FromBody] UpdateRideRequestDto rideRequestDto)
    {
        var result = await _mediator.Send(new UpdateRideRequestCommand { RideRequestDto = rideRequestDto ,UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideRequestCommand { Id = id });

        var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    
    [HttpPut("cancel/{id}")]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> CancelRideRequest(int id)
    {
        var result = await _mediator.Send(new UpdateRideRequestStatusCommand{ Id = id ,UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }
}
