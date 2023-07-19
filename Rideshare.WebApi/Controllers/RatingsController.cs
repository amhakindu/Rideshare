using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Features.Drivers.Queries;

namespace Rideshare.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/ratings")]
public class RatingsController : BaseApiController
{
	public RatingsController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
	{
	}

	[HttpGet("{id}")]
	[Authorize(Roles = "Commuter, Admin")]
	public async Task<IActionResult> Get(int id)
	{
		var result = await _mediator.Send(new GetRateDetailQuery { RateId = id });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	 
	[HttpGet]
	[Authorize(Roles = "Commuter, Admin")]
	public async Task<IActionResult> GetRatingsOfCurrentUser([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetRateListQuery { UserId = _userAccessor.GetUserId(), PageNumber=pageNumber, PageSize=pageSize });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}

	[HttpGet("of-user/{userId}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetRatingsOfGivenUser(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetRateListQuery { UserId = userId, PageNumber=pageNumber, PageSize=pageSize });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	[HttpGet("of-driver/{driverId}")]
	[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get(int driverId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetRatesByDriverIdRequest {PageNumber=pageNumber, PageSize=pageSize, DriverId = driverId });
        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }
	
	[HttpPost]
	[Authorize(Roles = "Commuter")]
	public async Task<IActionResult> Post([FromBody] CreateRateDto rateDto)
	{
		var result = await _mediator.Send(new CreateRateCommand { RateDto = rateDto, UserId= _userAccessor.GetUserId() });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}
	
	[HttpPut]
	[Authorize(Roles = "Commuter")]
	public async Task<IActionResult> Put([FromBody] UpdateRateDto rateDto)
	{
		var result = await _mediator.Send(new UpdateRateCommand { RateDto = rateDto, UserId= _userAccessor.GetUserId() });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Commuter")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _mediator.Send(new DeleteRateCommand { Id = id, UserId = _userAccessor.GetUserId() });
		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}
}
