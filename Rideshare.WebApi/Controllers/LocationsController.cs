using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Features.Locations.Queries;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/locations")]
public class LocationsController : BaseApiController
{
    public LocationsController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    [HttpGet("popular")]
    [Authorize(Roles = "Commuter, Driver")]
    public async Task<IActionResult> GetPopularDestinations([FromQuery] int limit)
    {
        var result = await _mediator.Send(new GetPopularDestinationsOfUserQuery{ Limit = limit, UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<IList<Dictionary<string, object>>>>(status, result);
    } 
}
