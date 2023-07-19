using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Features.RideOffers.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/rideoffers")]
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
    
    [HttpGet]
    [Authorize(Roles = "Driver")]
    [ProducesResponseType(typeof(PaginatedResponse<RideOfferListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersOfLoggedInDriver([FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetRideOffersOfDriverQuery { Id = _userAccessor.GetUserId(), PageNumber = PageNumber, PageSize = PageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("of-driver/{driverId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResponse<RideOfferListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersOfDriverWithId(int driverId, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetRideOffersOfDriverQuery { Id = driverId, PageNumber = PageNumber, PageSize = PageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResponse<RideOfferListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetAllRideOffersQuery(){ PageNumber= PageNumber, PageSize= PageSize});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<PaginatedResponse<RideOfferListDto>>(status, result);
    }


    [HttpGet("filter")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SearchAndFilter([FromQuery] RideOffersListFilterDto SearchDto, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new SearchAndFilterQuery { PageNumber = PageNumber, PageSize = PageSize, SearchDto = SearchDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
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
    
    [HttpPatch("cancel/{id}")]
    [Authorize(Roles = "Driver")]
    [ProducesResponseType(typeof(BaseResponse<Unit>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelRideOffer(int id)
    {
        var result = await _mediator.Send(new CancelRideOfferCommand { RideOfferId= id , UserId=_userAccessor.GetUserId()});
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Unit>>(status, result);
    }
}
