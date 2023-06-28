using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Features.Userss;
using System.Web.Http;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Common.Dtos.Statistics;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class BaseApiController : ControllerBase
{
    protected readonly IUserAccessor _userAccessor;
    protected readonly IMediator _mediator;

    public BaseApiController(IMediator mediator, IUserAccessor userAccessor)
    {
        _mediator = mediator;
        _userAccessor = userAccessor;
    }

    [Microsoft.AspNetCore.Mvc.HttpGet("Statistics/Week/PercentageChange")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<IList<EntityCountChangeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPercentageChangeFromLastWeek()
    {
        var result = await _mediator.Send(new GetPercentageChangeFromLastWeekQuery{});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<IList<EntityCountChangeDto>>>(status, result);
    } 


    public ActionResult getResponse<T>(HttpStatusCode status, T? payload){

        if(status == HttpStatusCode.Created){
            return Created("", payload);
        }else if(status == HttpStatusCode.BadRequest){
            return BadRequest(payload);
        }else if(status == HttpStatusCode.OK){
            return Ok(payload);
        }else if(status == HttpStatusCode.NotFound){
            return NotFound(payload);
        }else if(status == HttpStatusCode.Accepted){
            return Accepted(payload);
        }
        else if(status == HttpStatusCode.Unauthorized){
            return Unauthorized(payload);
        }
        return NoContent();
    }
}
