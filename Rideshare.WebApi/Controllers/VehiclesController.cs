using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Features.Vehicles.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/vehicles")]
public class VehiclesController : BaseApiController
{
    public VehiclesController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Commuter,Driver,Admin")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetVehicleQuery { VehicleId = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Commuter,Driver,Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllVehiclesQuery { PageNumber = pageNumber, PageSize = pageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Driver")]
    public async Task<IActionResult> Post([FromForm] CreateVehicleDto createVehicleDto)
    {
        var result = await _mediator.Send(new CreateVehicleCommand { VehicleDto = createVehicleDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [HttpPatch]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Patch([FromBody] UpdateVehicleDto updateVehicleDto)
    {
        var result = await _mediator.Send(new UpdateVehicleCommand { VehicleDto = updateVehicleDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteVehicleCommand { VehicleId = id });

        var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}
