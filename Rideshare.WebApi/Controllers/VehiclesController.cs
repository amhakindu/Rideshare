using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Responses;
using System.Net;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class VehiclesController : BaseApiController
{
    public VehiclesController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {}
    public IUnitOfWork _unitOfWork { get; set; }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Commuter,Driver,Admin")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetVehicleQuery { VehicleId = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    [HttpGet("NumberOfVehicle")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetNumberOfVihcle([FromQuery] string option, [FromQuery] int year, [FromQuery] int month)
    {
        var result = await _mediator.Send(new GetNumberOfVehicleQuery { option = option, Year = year, Month = month });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet]
    [Authorize(Roles = "Commuter,Driver,Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllVehiclesQuery { PageNumber = pageNumber, PageSize = pageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
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
