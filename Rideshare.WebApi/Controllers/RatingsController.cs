using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Features.Drivers.Queries;

namespace Rideshare.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/ratings")]
public class RatingsController : BaseApiController
{
	public RatingsController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
	{
	}
	
	/// <summary>
	/// Get details of a specific rating.
	/// </summary>
	/// <remarks> Retrieves details of a specific rating based on its unique identifier.
	/// This endpoint is accessible to "Commuter" and "Admin" users.
	/// Provides information such as Rating Id, Rating value, Comment, User who rated, and more.
	///
	/// Sample Response:
	/// {
	///    "success": true,
	///	   "message": "Get Successful",
	///	   "value": {
	///		 "id": 1,
	///		 "userId": "23c4d56e-78f9-40a1-23c4-123456789026",
	///		 "rate": 8.9,
	///		 "driverId": 1,
	///		 "description": "Great service!"
	///	     },
	///    "errors": []
	/// }</remarks> 
	/// <param name="id">Unique identifier of the rating.</param>
	/// <response code="200">Returns the details of the specified rating.</response>
	/// <response code="404">Rating not found.</response>
	/// <returns>
	/// The details of the specified rating.
	/// </returns>	

	[HttpGet("{id}")]
	[Authorize(Roles = "Commuter, Admin")]
	public async Task<IActionResult> Get(int id)
	{
		var result = await _mediator.Send(new GetRateDetailQuery { RateId = id });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	 
	/// <summary>
	/// Get ratings of the current user.
	/// </summary>
	/// <remarks>Retrieves a paginated list of all the ratings posted in the ride-share system.
	/// This endpoint is accessible to "Commuter" and "Admin" users.
	///
	///Sampple Response:
	/// {
	///	"pageNumber": 1,
	///	"pageSize": 10,
	///	"count": 10000,
	///	"success": true,
	///	"message": "Fetch Succesful",
	///	"value": [
	///		{
	///		"id": 1,
	///		"userId": "23c4d56e-78f9-40a1-23c4-123456789026",
	///		"rate": 8.9,
	///		"driverId": 1,
	///		"description": "Great service!"
	///		},
	///		...9 other ratings
	///        ]
	///} </remarks>
	/// <response code="200">Returns the paginated list of ratings.</response>
	/// <returns>A paginated collection of ratings given by the current user.</returns>
		
	[HttpGet("all")]
	[Authorize(Roles = "Commuter, Admin")]
	public async Task<IActionResult> GetRatingsOfCurrentUser([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetRateListQuery { UserId = _userAccessor.GetUserId(), PageNumber=pageNumber, PageSize=pageSize });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Get ratings of a specific user.
	/// </summary>
	/// <remarks>Retrieves a paginated list of ratings posted by a specific user with a given UserId.
	/// This feature is accessible to "Admin" users Only.
	/// </remarks>
	/// <response code="200">Returns the paginated list of ratings.</response>
	/// <returns>A paginated collection of ratings given to the specified user.</returns>
	

	[HttpGet("of-user/{userId}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetRatingsOfGivenUser(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetRateListQuery { UserId = userId, PageNumber=pageNumber, PageSize=pageSize });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Get ratings of a specific driver.
	/// </summary>
	/// <remarks>Retrieves a paginated list of ratings given to a driver based on their unique identifier.
	/// This endpoint is accessible to "Admin" users.
	/// </remarks>
	/// <response code="200">Returns the paginated list of ratings.</response>
	/// <returns>A paginated collection of ratings given to the specified driver.</returns>
	   
	[HttpGet("of-driver/{driverId}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Get(int driverId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetRatesByDriverIdRequest {PageNumber=pageNumber, PageSize=pageSize, DriverId = driverId });
		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Post a new rating.
	/// </summary>
	/// <remarks>Posts a new rating for a driver (by commuters).
	/// This endpoint is accessible to users with the "Commuter" role.
	/// </remarks>
	/// <param name="rateDto">Rating information.</param>
	/// <returns>Details of the newly posted rating.</returns>
		
	[HttpPost]
	[Authorize(Roles = "Commuter")]
	public async Task<IActionResult> Post([FromBody] CreateRateDto rateDto)
	{
		var result = await _mediator.Send(new CreateRateCommand { RateDto = rateDto, UserId= _userAccessor.GetUserId() });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Update an existing rating.
	/// </summary>
	/// <remarks>Updates an existing rating given by the current user.
	/// This endpoint is accessible to users with the "Commuter" role.
	/// </remarks>
	/// <param name="rateDto">Updated rating information.</param>
	/// <returns>Details of the updated rating.</returns>
   
	[HttpPut]
	[Authorize(Roles = "Commuter")]
	public async Task<IActionResult> Put([FromBody] UpdateRateDto rateDto)
	{
		var result = await _mediator.Send(new UpdateRateCommand { RateDto = rateDto, UserId= _userAccessor.GetUserId() });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Delete a rating.
	/// </summary>
	/// <remarks>Deletes a specific rating based on its ID.
	/// This feature is accessible to users with the "Commuter" role.
	/// </remarks>
	/// <returns>A message indicating the result of the delete operation.</returns>
				 
	[HttpDelete("{id}")]
	[Authorize(Roles = "Commuter")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _mediator.Send(new DeleteRateCommand { Id = id, UserId = _userAccessor.GetUserId() });
		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}
}
