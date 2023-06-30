using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Features.Commuters.Queries;
using Rideshare.Application.Features.Userss;
namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommuterController : BaseApiController
{

	private readonly IUserAccessor _userAccessor;
	public CommuterController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
	{
		_userAccessor = userAccessor;
	}
	
	[HttpGet("commuter-status")]
	[AllowAnonymous]
	public async Task<IActionResult> GetCommuterStatus()
	{
		var result = await _mediator.Send(new GetCommuterStatusQuery());

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	

	[AllowAnonymous]
	[HttpGet("commuter-count")]
	public async Task<IActionResult> GetCommuterCount([FromQuery] string option, [FromQuery] int? year, [FromQuery] int? month)
	{
		switch (option.ToLower())
		{
			case "yearly":
				var yearlyResult = await _mediator.Send(new GetYearlyCommuterCountQuery());
				var yearlyStatus = yearlyResult.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
				return getResponse(yearlyStatus, yearlyResult);

			case "monthly":
				var monthlyResult = await _mediator.Send(new GetMonthlyCommuterCountQuery {Year = year ?? 0});
				var monthlyStatus = monthlyResult.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
				return getResponse(monthlyStatus, monthlyResult);

			case "weekly":
				var weeklyResult = await _mediator.Send(new GetWeeklyCommuterCountQuery { Year = year ?? 0, Month = month ?? 0 });
				var weeklyStatus = weeklyResult.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
				return getResponse(weeklyStatus, weeklyResult);

			default:
				throw new ArgumentException("Invalid option provided.");
		}
	} 
 
}

