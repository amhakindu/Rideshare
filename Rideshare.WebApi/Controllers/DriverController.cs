using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Features.Drivers.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/drivers")]
[Authorize]
public class DriverController : BaseApiController
{
	public DriverController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
	{
	}
	
	/// <summary>
	/// Get a paginated list of drivers.
	/// </summary>
	/// <remarks> Returns a paginated list of drivers registered for the ride-share system, accessible to "Admin" users.
	/// Provides efficient navigation through available drivers, with details such as Id, UserId, User Detail, Experience, Address and more.
	///
	/// Sample Response:
	/// {
	///    "pageNumber": 1,
	///	   "pageSize": 10,
	///	   "count": 15,
	///	   "success": true,
	///	   "message": "Fetch Successful",	
	///     "value": [	
	///	          10 drivers with details...
	///           ]
	/// }</remarks>
	/// <response code="200">Returns the paginated list of drivers.</response>
	/// <returns>
	/// A paginated collection of drivers.
	/// </returns>
	
	[HttpGet("all")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetDriversListRequest { PageNumber=pageNumber, PageSize = pageSize });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Get details of a specific driver.
	/// </summary>
	/// <remarks> Retrieves details of a specific driver based on their unique identifier.
	/// This endpoint is accessible to all users.
	///
	/// Sample Response:
	/// {
	///    "success": true,
	///    "message": "Fetch Successful",
	///    "value": {	
	///	        "Id" : 1, 
	///         "userId": "2e4c2829-138d-4e7e-b023-123456789105",
	///	   		"rate": [total, count, average],
	///	  		"experience": 3.5,
	///	   		"address": "Lafto",
	///    		"license": "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png",
	///    		"verified": true
	///    },
	///    "errors": []
	/// }</remarks> 
	/// <response code="200">Returns the paginated list of drivers.</response>
	/// <returns>
	/// The details of the specified driver.
	/// </returns>
	
	[HttpGet("{id}")]
	[AllowAnonymous]
	public async Task<IActionResult> Get(int id)
	{
		var result = await _mediator.Send(new GetDriverRequest { Id = id });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Get driver information by user ID.
	/// </summary>
	/// <remarks> Retrieves information about a driver based on the associated user ID.
	/// This endpoint is accessible to users with the "Driver" or "Admin" roles.
	///
	/// Sample Response:
	/// {
	///    "success": true,
	///    "message": "Fetch Successful",
	///    "value": {	
	///	        "Id" : 1, 
	///         "userId": "2e4c2829-138d-4e7e-b023-123456789105",
	///	   		"rate": [total, count, average],
	///	  		"experience": 3.5,
	///	   		"address": "Lafto",
	///    		"license": "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png",
	///    		"verified": true
	///    },
	///    "errors": []
	/// }</remarks> 
	/// <param name="userId">User's unique identifier associated with the driver.</param>
	/// <response code="200">Returns the driver's information.</response>
	/// <response code="404">Driver not found.</response>
	/// <returns>
	/// Information about the driver associated with the provided user ID.
	/// </returns>
	
	[HttpGet("of-user/{userId}")]
	[Authorize(Roles ="Driver,Admin")]
	public async Task<IActionResult> GetUser(string userId)
	{
		var result = await _mediator.Send(new GetDriverByUserIdQuery { Id = userId });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Register a new driver.
	/// </summary>
	/// <remarks>Registers a new driver in the system.
	/// This endpoint is accessible to users with the "Driver" role.
	/// It allows users to provide driver information for registration.
	/// </remarks>
	/// <param name="createDriverDto">Driver's information for registration.</param>
	/// <returns>
	/// Details of the newly registered driver.
	/// </returns>ponse code="201">Returns the newly registered driver's details.
   
	[HttpPost]
	[Authorize(Roles = "Driver")]
	public async Task<IActionResult> Post([FromForm] CreateDriverDto createDriverDto)
	{
		var result = await _mediator.Send(new CreateDriverCommand { CreateDriverDto = createDriverDto, UserId = _userAccessor.GetUserId() });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}

	/// <summary>
	/// Delete a driver.
	/// </summary>
	/// <remarks>
	/// Deletes a driver based on the unique identifier.
	/// This endpoint is accessible to users with the "Driver" role.
	/// It allows users to delete their own driver profile.
	/// </remarks>
	/// <param name="id">Driver's unique identifier.</param>
	/// <response code="200">Driver deleted successfully.</response>
	/// <response code="401">Unauthorized access to delete the driver.</response>
	/// <response code="404">Driver not found.</response>
	/// <returns>
	/// A message indicating the result of the delete operation.
	/// </returns>
	
	[HttpDelete("{id}")]
	[Authorize(Roles = "Driver")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _mediator.Send(new DeleteDriverCommand { Id = id , UserId = _userAccessor.GetUserId()});

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}

	/// <summary>
	/// Update driver information.
	/// </summary>
	/// <remarks>
	/// Updates the information of an existing driver.
	/// This endpoint is accessible to users with the "Driver" role.
	/// Users can update their own driver profile information using this endpoint.
	/// </remarks>
	/// <param name="updateDriverDto">Driver's updated information.</param>
	/// <response code="200">Driver information updated successfully.</response>
	/// <response code="400">Bad request or invalid input data.</response>
	/// <response code="401">Unauthorized access to update driver information.</response>
	/// <response code="404">Driver not found.</response>
	/// <returns>
	/// A message indicating the result of the update operation.
	/// </returns><response code="200">Driver information updated successfully.</response>
		
	[HttpPut]
	[Authorize(Roles = "Driver")]
	public async Task<IActionResult> Update([FromBody] UpdateDriverDto updateDriverDto)
	{
		var result = await _mediator.Send(new UpdateDriverCommand { UpdateDriverDto = updateDriverDto , UserId = _userAccessor.GetUserId()});

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}

	/// <summary>
	/// Approve a driver.
	/// </summary>
	/// <remarks>
	/// Approves a driver registration request.
	/// This endpoint is accessible to users with the "Admin" role.
	/// It allows administrators to review and approve pending driver registration requests.
	/// </remarks>
	/// <param name="approveDriverDto">Driver's information for approval.</param>
	/// <response code="200">Driver approved successfully.</response>
	/// <response code="400">Bad request or invalid input data.</response>
	/// <response code="401">Unauthorized access to approve a driver.</response>
	/// <response code="404">Driver not found or registration request not found.</response>
	/// <returns>
	/// A message indicating the result of the approval operation.
	/// </returns>
	
	[HttpPut("approve/{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Approve([FromBody] ApproveDriverDto approveDriverDto)
	{
		var result = await _mediator.Send(new ApproveDriverCommand { ApproveDriverDto = approveDriverDto });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
}