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
    /// <summary>
    /// Get Vehicle information by user ID.
    /// </summary>
    /// <remarks> Retrieves information about a vehicle based on the associated ID.
    /// This endpoint is accessible to users with the "Vehicle" or "Admin" roles.
    ///
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Vehicle Fetch Success",
    ///    "value": {	
    ///	        "Id" : 1, 
    ///         "plateNumber": "ABC123",
    ///	   		"numberOfSeats": 40,
    ///	  		"model": "Toyota Camry",
    ///	   		"address": "Lafto",
    ///    		"libre": "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100796/xsabqbbff2jc7kqc6efx.pdf",
    ///    		 "driverId": 1
    ///    },
    ///    "errors": []
    /// }</remarks> 
    /// <param name="userId">User's unique identifier associated with the Vehicle.</param>
    /// <response code="200">Returns the vehicle's information.</response>
    /// <response code="404">Vehicle not found.</response>
    /// <returns>
    /// Information about the vehicle associated with the provided ID.
    /// </returns>


    [HttpGet("{id}")]
    [Authorize(Roles = "Commuter,Driver,Admin")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetVehicleQuery { VehicleId = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }


    /// <summary>
	/// Get a list of all vehicles.
	/// </summary>
	/// <remarks> Returns a list of vehicles registered for the ride-share system, accessible to "Admin" users.
	/// Provides efficient navigation through available vehicles, with details such as Id, driverId, plateNumber, numberOfSeats, model, libre and more.
	///
	/// Sample Response:
	/// {
	///    "pageNumber": 1,
	///	   "pageSize": 10,
	///	   "count": 8,
	///	   "success": true,
	///	   "message": "Vehicles Fetching Successful",	
	///     "value": [	
	///	          10 vehicles with details...
	///           ]
	/// }</remarks>
	/// <response code="200"> Returns the list of vehicles.</response>
	/// <returns>
	/// A list of all drivers. 
	/// </returns>
    [HttpGet("all")]
    [Authorize(Roles = "Commuter,Driver,Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllVehiclesQuery { PageNumber = pageNumber, PageSize = pageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }


    /// <summary>
	/// Register a new vehicle.
	/// </summary>
	/// <remarks>Registers a new vehicle in the system.
	/// This endpoint is accessible to users with the "Vehicle" role.
	/// It allows users to provide vehicle information for registration.
	/// </remarks>
	/// <param name="createVehicleDto">Vehicle's information for registration.</param>
	/// <returns>
	/// Details of the newly registered vehicle.
	/// </returns>ponse code="201">Returns the newly registered vehicle's details.
    [HttpPost]
    [Authorize(Roles = "Admin, Driver")]
    public async Task<IActionResult> Post([FromForm] CreateVehicleDto createVehicleDto)
    {
        var result = await _mediator.Send(new CreateVehicleCommand { VehicleDto = createVehicleDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }


    /// <summary>
	/// Update vehicle information.
	/// </summary>
	/// <remarks>
	/// Updates the information of an existing vehicle.
	/// This endpoint is accessible to users with the "Vehicle" role.
	/// Users can update their own vehicle profile information using this endpoint.
	/// </remarks>
	/// <param name="updateVehicleDto">Vehicle's updated information.</param>
	/// <response code="200">Vehicle information updated successfully.</response>
	/// <response code="400">Bad request or invalid input data.</response>
	/// <response code="401">Unauthorized access to update vehicle information.</response>
	/// <response code="404">Vehicle not found.</response>
	/// <returns>
	/// A message indicating the result of the update operation.
	/// </returns><response code="200">Vehicle information updated successfully.</response>
    [HttpPatch]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Patch([FromBody] UpdateVehicleDto updateVehicleDto)
    {
        var result = await _mediator.Send(new UpdateVehicleCommand { VehicleDto = updateVehicleDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }


    /// <summary>
	/// Delete a vehicle.
	/// </summary>
	/// <remarks>
	/// Deletes a vehicle based on the unique identifier.
	/// This endpoint is accessible to users with the "Vehicle" role.
	/// It allows users to delete their own vehicle profile.
	/// </remarks>
	/// <param name="id">Vehicle's unique identifier.</param>
	/// <response code="200">Vehicle deleted successfully.</response>
	/// <response code="401">Unauthorized access to delete the vehicle.</response>
	/// <response code="404">Vehicle not found.</response>
	/// <returns>
	/// A message indicating the result of the delete operation.
	/// </returns>

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteVehicleCommand { VehicleId = id });

        var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
}
