using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RideOffersController : BaseApiController
{
    public RideOffersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateRideOfferDto createRideOfferDto)
    {
        var result = await _mediator.Send(new CreateRideOfferCommand { RideOfferDto = createRideOfferDto });
        
        var status = result.Success ? HttpStatusCode.Created: HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<int>>(status, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideOfferWithDetailsQuery { RideOfferID = id });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<RideOfferDto>>(status, result);
    }
    [HttpGet("NoRideOfferForTop10Model")]
    public async Task<IActionResult> GetNumberOfVihcle()
    {
        var result = await _mediator.Send(new GetNoTopModelRideOffferQuery {});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetRideOffers([FromQuery]string? DriverId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        BaseResponse<IReadOnlyList<RideOfferListDto>> result;
        if(DriverId != null){
            result = await _mediator.Send(new GetRideOffersQuery { DriverID = DriverId, PageNumber = pageNumber, PageSize = pageSize });
        }else{
            result = await _mediator.Send(new GetAllRideOffersQuery{});
        }

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<IReadOnlyList<RideOfferListDto>>>(status, result);
    } 

    [HttpPatch]  
    public async Task<IActionResult> Patch([FromBody] UpdateRideOfferDto updateRideOfferDto){
        var result = await _mediator.Send(new UpdateRideOfferCommand { RideOfferDto = updateRideOfferDto });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.Accepted;
        return getResponse<BaseResponse<Unit>>(status, result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideOfferCommand { RideOfferID = id });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Unit>>(status, result);
    }
}
