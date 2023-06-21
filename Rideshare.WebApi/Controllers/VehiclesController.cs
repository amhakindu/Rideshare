using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Responses;
using System.Net;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : BaseApiController
{
    public IUnitOfWork _unitOfWork { get; set; }
    public VehiclesController(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetVehicleQuery { VehicleId = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllVehiclesQuery { PageNumber = pageNumber, PageSize = pageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CreateVehicleDto createVehicleDto)
    {
        var result = await _mediator.Send(new CreateVehicleCommand { VehicleDto = createVehicleDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromBody] UpdateVehicleDto updateVehicleDto)
    {
        var result = await _mediator.Send(new UpdateVehicleCommand { VehicleDto = updateVehicleDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteVehicleCommand { VehicleId = id });

        var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}
