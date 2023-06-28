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
using Rideshare.Application.Common.Dtos.Drivers;

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
    [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateRideOfferDto createRideOfferDto)
    {
        
        Console.WriteLine($" LONG CURRENT{createRideOfferDto.CurrentLocation.Longitude}");
        Console.WriteLine($"latURRENT {createRideOfferDto.CurrentLocation.Latitude}");
        Console.WriteLine(createRideOfferDto.Destination.Longitude);
        Console.WriteLine(createRideOfferDto.Destination.Latitude);
        
        var result = await _mediator.Send(new CreateRideOfferCommand { RideOfferDto = createRideOfferDto, UserId = _userAccessor.GetUserId() });
        
        var status = result.Success ? HttpStatusCode.Created: HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<int>>(status, result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Driver,Admin")]
    [ProducesResponseType(typeof(BaseResponse<RideOfferDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideOfferWithDetailsQuery { RideOfferID = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<RideOfferDto>>(status, result);
    }
    [HttpGet("NoRideOfferForTop10Model")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<IReadOnlyList<ModelAndCountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNumberOfVihcle()
    {
        var result = await _mediator.Send(new GetNoTopModelRideOffferQuery { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet]
    [Authorize(Roles = "Driver")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<string, IReadOnlyList<RideOfferListDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDriverRideOffers([FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetRideOffersOfDriverQuery { UserId = _userAccessor.GetUserId(), PageNumber = PageNumber, PageSize = PageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("Search")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<string, IReadOnlyList<RideOfferListDto>>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<string, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAndFilter([FromQuery]SearchAndFilterDto SearchDto, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new SearchAndFilterQuery { PageNumber = PageNumber, PageSize = PageSize, SearchDto = SearchDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("Statistics")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<int, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats([FromQuery]RideOfferStatsDto StatsDto, [FromQuery] string options)
    {
        var result = await _mediator.Send(new GetRideOfferStatsQuery { StatsDto = StatsDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<int, int>>>(status, result);
    }

    [HttpGet("Statistics/Status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<int, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatsWithStatus([FromQuery]RideOfferStatsDto StatsDto, [FromQuery] string options)
    {
        var result = await _mediator.Send(new GetRideOfferStatsWithStatusQuery{StatsDto = StatsDto});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, Dictionary<int, int>>>>(status, result);
    } 

    [HttpGet("Statistics/Status/Count")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<string, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountForEachStatus()
    {
        var result = await _mediator.Send(new GetCountForEachStatusQuery{});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, int>>>(status, result);
    } 

    [HttpPatch]  
    [Authorize(Roles = "Driver,Admin")]
    [ProducesResponseType(typeof(BaseResponse<Unit>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch([FromBody] UpdateRideOfferDto updateRideOfferDto){
        var result = await _mediator.Send(new UpdateRideOfferCommand { RideOfferDto = updateRideOfferDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.Accepted;
        return getResponse<BaseResponse<Unit>>(status, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<Unit>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideOfferCommand { RideOfferID = id });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Unit>>(status, result);
    }
    
    [HttpPatch("Cancel/{id}")]
    [Authorize(Roles = "Driver")]
    [ProducesResponseType(typeof(BaseResponse<Unit>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelRideOffer(int id)
    {
        var result = await _mediator.Send(new CancelRideOfferCommand { RideOfferId= id , UserId=_userAccessor.GetUserId()});
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Unit>>(status, result);
    }

    [HttpGet("TopFiveDrivers")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<List<DriverStatsDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopFiveDrivers()
    {
        var result = await _mediator.Send(new GetTopDriversWithStatsRequest { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}
