using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Responses;
using System.Net;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RateController : BaseApiController
{
	public RateController(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator, unitOfWork)
	{
	}


	[HttpGet] //api/Rates
	public async Task<IActionResult> GetAll()
	{
		var result = await _mediator.Send(new GetRateListQuery());

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id)
	{
		var result = await _mediator.Send(new GetRateDetailQuery { RateId = id });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] CreateRateDto createRateDto)
	{
		var result = await _mediator.Send(new CreateRateCommand { RateDto = createRateDto });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}

   [HttpPut]
	public async Task<IActionResult> Put([FromBody] UpdateRateDto updateRateDto)
	{
		var result = await _mediator.Send(new UpdateRateCommand { RateDto = updateRateDto });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}

	[HttpDelete]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _mediator.Send(new DeleteRateCommand { RateId = id });

		var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
}
