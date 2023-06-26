using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Features.Userss;
using System.Net;

namespace Rideshare.WebApi.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RateController : BaseApiController
{
	public RateController(IMediator mediator,IUserAccessor userAccessor) : base(mediator, userAccessor)
	{
	}
	 
	  [AllowAnonymous]
	  [HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var result = await _mediator.Send(new GetRateListQuery { UserId = _userAccessor.GetUserId(), PageNumber=pageNumber, PageSize=pageSize });
			var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
			return getResponse(status, result);
		}
		
		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _mediator.Send(new GetRateDetailQuery { RateId = id });
			var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
			return getResponse(status, result);
		}

		[Authorize(Roles = "Commuter")]
		[HttpPost]
		
		public async Task<IActionResult> Post([FromBody] CreateRateDto rateDto)
		{
			var result = await _mediator.Send(new CreateRateCommand { RateDto = rateDto });

			var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
			return getResponse(status, result);
		}
		
		[Authorize(Roles = "Commuter")]
		[HttpPut]
		
		public async Task<IActionResult> Put([FromBody] UpdateRateDto rateDto)
		{
			var result = await _mediator.Send(new UpdateRateCommand { RateDto = rateDto });

			var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
			return getResponse(status, result);
		}

		[Authorize(Roles = "Commuter")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int id, string userId)
		{
			var result = await _mediator.Send(new DeleteRateCommand { Id = id, UserId = userId });
			var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
			return getResponse(status, result);
		}

	[HttpGet("driver/{driverId}")]
    public async Task<IActionResult> Get( int driverId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetRatesByDriverIdRequest {PageNumber=pageNumber, PageSize=pageSize, DriverId = driverId });
        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }
}
