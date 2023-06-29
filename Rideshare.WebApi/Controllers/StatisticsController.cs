using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Responses;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class StatisticsController : BaseApiController
{
    public StatisticsController(IMediator mediator, IUserAccessor userAccessor): base(mediator, userAccessor)
    {
    }

    [HttpGet("Week/PercentageChange")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<IList<EntityCountChangeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPercentageChangeFromLastWeek()
    {
        var result = await _mediator.Send(new GetPercentageChangeFromLastWeekQuery{});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<IList<EntityCountChangeDto>>>(status, result);
    } 
}
