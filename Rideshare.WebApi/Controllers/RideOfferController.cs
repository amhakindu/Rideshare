using System.Collections.Generic;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Userss;
using Microsoft.AspNetCore.Authorization;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RideOffersController : BaseApiController
{
    public RideOffersController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    [HttpPost]
    [Authorize(Roles = "Driver")]
    public async Task<IActionResult> Post([FromBody] CreateRideOfferDto createRideOfferDto)
    {
        var result = await _mediator.Send(new CreateRideOfferCommand { RideOfferDto = createRideOfferDto });
        
        var status = result.Success ? HttpStatusCode.Created: HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<int>>(status, result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Driver,Admin")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideOfferWithDetailsQuery { RideOfferID = id });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<RideOfferDto>>(status, result);
    }
    [HttpGet("NoRideOfferForTop10Model")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetNumberOfVihcle()
    {
        var result = await _mediator.Send(new GetNoTopModelRideOffferQuery {});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet]
    [Authorize(Roles = "Driver")]
    public async Task<IActionResult> GetDriverRideOffers([FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetRideOffersOfDriverQuery {UserId=_userAccessor.GetUserId(), PageNumber=PageNumber, PageSize=PageSize});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, object>>>(status, result);
    } 

    [HttpGet("Search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SearchAndFilter([FromQuery]SearchAndFilterDto SearchDto, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new SearchAndFilterQuery{ PageNumber=PageNumber, PageSize=PageSize, SearchDto = SearchDto});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, object>>>(status, result);
    } 

    [HttpGet("Statistics")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStats([FromQuery]RideOfferStatsDto StatsDto)
    {
        var result = await _mediator.Send(new GetRideOfferStatsQuery{StatsDto = StatsDto});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<int, int>>>(status, result);
    } 

    [HttpGet("Statistics/Status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStatsWithStatus([FromQuery]RideOfferStatsDto StatsDto)
    {
        var result = await _mediator.Send(new GetRideOfferStatsWithStatusQuery{StatsDto = StatsDto});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, Dictionary<int, int>>>>(status, result);
    } 

    [HttpPatch]  
    [Authorize(Roles = "Driver,Admin")]
    public async Task<IActionResult> Patch([FromBody] UpdateRideOfferDto updateRideOfferDto){
        var result = await _mediator.Send(new UpdateRideOfferCommand { RideOfferDto = updateRideOfferDto });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.Accepted;
        return getResponse<BaseResponse<Unit>>(status, result);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Driver,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideOfferCommand { RideOfferID = id });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Unit>>(status, result);
    }


    [HttpGet("TopFiveDrivers")]
    public async Task<IActionResult> GetTopFiveDrivers()
    {
        var result = await _mediator.Send(new GetTopDriversWithStatsRequest { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}
