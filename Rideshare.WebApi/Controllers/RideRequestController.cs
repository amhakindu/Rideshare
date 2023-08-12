using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Features.RideRequests.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/riderequests")]

public class RideRequestController : BaseApiController
{
    public RideRequestController(IMediator mediator,IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    /// <summary>
    /// Get a ride request by ID
    /// </summary>
    /// <remarks>Retrieves a specific ride request based on its unique identifier.
    /// This endpoint is accessible to commuters and admins.
    ///
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride request retrieved successfully",
    ///    "value": {
    ///        "id": 123,
    ///        "origin": {
    ///            "latitude": 12.3456,
    ///            "longitude": -78.9012
    ///        },
    ///        "destination": {
    ///            "latitude": 34.5678,
    ///            "longitude": -56.7890
    ///        },
    ///        "originAddress": "123 Main St",
    ///        "destinationAddress": "456 Elm St",
    ///        "currentFare": 25.0,
    ///        "status": "Pending",
    ///        "numberOfSeats": 2,
    ///        "user": {
    ///            "fullName": "John Doe",
    ///            "phoneNumber": "123-456-7890",
    ///            "age": 30,
    ///            "statusByLogin": "Active",
    ///            "profilePicture": "http://example.com/profile.jpg"
    ///        },
    ///        "accepted": false
    ///    },
    ///    "errors": []
    /// }
    /// </remarks>
    /// <param name="id">The unique identifier of the ride request to retrieve</param>
    /// <response code="200">Indicates that the ride request was successfully retrieved</response>
    /// <response code="404">Indicates that the specified ride request was not found</response>
    /// <returns>
    /// A response containing the retrieved ride request information.
    /// If successful, the response will include the details of the ride request; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Commuter, Admin")]
    [ProducesResponseType(typeof(BaseResponse<RideRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideRequestQuery { Id = id,UserId = _userAccessor.GetUserId() });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<RideRequestDto>>(status, result);
    }

    /// <summary>
    /// Search and filter ride requests
    /// </summary>
    /// <remarks> Retrieves a paginated list of ride requests based on search criteria.
    /// This endpoint is accessible to Admins only.
    ///
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride requests fetching successful",
    ///    "value": [
    ///           {
    ///                "id": 124,
    ///                "origin": {
    ///                    "latitude": 10.1111,
    ///                    "longitude": -70.2222
    ///                },
    ///                "destination": {
    ///                    "latitude": 30.3333,
    ///                    "longitude": -50.4444
    ///                },
    ///                ...
    ///                "accepted": true
    ///            },
    ///            ...
    ///    ],
    ///    "pageNumber": 1,
    ///    "pageSize": 10,
    ///    "count": 2,
    ///    "errors": []
    /// }</remarks>
    /// <param name="RideRequestsListFilterDto">Search filter parameters</param>
    /// <param name="PageNumber">Page number (default: 1)</param>
    /// <param name="PageSize">Number of items per page (default: 10)</param>
    /// <response code="200">Riderequests fetching successful</response>
    /// <response code="406">Invalid search filter parameters </response>
    /// <returns>A list of ride offers</returns>
    [HttpGet("filter")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResponse<RideRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommuterRequests([FromQuery] RideRequestsListFilterDto RideRequestsListFilterDto,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
     var result = await _mediator.Send(new GetRideRequestListQuery{ RideRequestsListFilterDto = RideRequestsListFilterDto,PageNumber=pageNumber, PageSize = pageSize});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Get ride requests of the authenticated commuter
    /// </summary>
    /// <remarks>Retrieves a paginated list of ride requests associated with the currently authenticated commuter.
    /// This endpoint is accessible to authorized commuters only.
    ///
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride requests fetching successful",
    ///    "value": [
    ///           {
    ///                "id": 124,
    ///                "origin": {
    ///                    "latitude": 10.1111,
    ///                    "longitude": -70.2222
    ///                },
    ///                "destination": {
    ///                    "latitude": 30.3333,
    ///                    "longitude": -50.4444
    ///                },
    ///                ...
    ///                "accepted": true
    ///            },
    ///            ...
    ///    ],
    ///    "pageNumber": 1,
    ///    "pageSize": 10,
    ///    "count": 2,
    ///    "errors": []
    /// }
    /// </remarks>
    /// <param name="pageNumber">Page number for pagination (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10)</param>
    /// <response code="200">Indicates that the ride requests were successfully retrieved</response>
    /// <returns>
    /// A response containing the paginated list of ride requests.
    /// If successful, the response will include the list of ride requests; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> GetRideRequestsOfLoggedInCommuter([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
     var result = await _mediator.Send(new GetRideRequestUserQuery {PageNumber=pageNumber, PageSize = pageSize,UserId = _userAccessor.GetUserId() });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Get ride requests of a specific user
    /// </summary>
    /// <remarks>Retrieves a paginated list of ride requests associated with a specific user based on their unique identifier.
    /// This endpoint is accessible to administrators only.
    ///
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride requests fetching successful",
    ///    "value": [
    ///           {
    ///                "id": 124,
    ///                "origin": {
    ///                    "latitude": 10.1111,
    ///                    "longitude": -70.2222
    ///                },
    ///                "destination": {
    ///                    "latitude": 30.3333,
    ///                    "longitude": -50.4444
    ///                },
    ///                ...
    ///                "accepted": true
    ///            },
    ///            ...
    ///    ],
    ///    "pageNumber": 1,
    ///    "pageSize": 10,
    ///    "count": 2,
    ///    "errors": []
    /// }
    /// </remarks>
    /// <param name="userId">The unique identifier of the user to retrieve ride requests for</param>
    /// <param name="pageNumber">Page number for pagination (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10)</param>
    /// <response code="200">Indicates that the ride requests were successfully retrieved</response>
    /// <returns>
    /// A response containing the paginated list of ride requests.
    /// If successful, the response will include the list of ride requests; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("of-commuter/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRideRequestsOfUserWithId(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
     var result = await _mediator.Send(new GetRideRequestUserQuery {PageNumber=pageNumber, PageSize = pageSize, UserId= userId });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Get all ride requests
    /// </summary>
    /// <remarks>Retrieves a paginated list of all ride requests in the system.
    /// This endpoint is accessible to administrators only.
    ///
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride requests fetching successful",
    ///    "value": [
    ///           {
    ///                "id": 124,
    ///                "origin": {
    ///                    "latitude": 10.1111,
    ///                    "longitude": -70.2222
    ///                },
    ///                "destination": {
    ///                    "latitude": 30.3333,
    ///                    "longitude": -50.4444
    ///                },
    ///                ...
    ///                "accepted": true
    ///            },
    ///            ...
    ///    ],
    ///    "pageNumber": 1,
    ///    "pageSize": 10,
    ///    "count": 2,
    ///    "errors": []
    /// }
    /// </remarks>
    /// <param name="pageNumber">Page number for pagination (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10)</param>
    /// <response code="200">Indicates that the ride requests were successfully retrieved</response>
    /// <response code="404">Indicates that no ride requests were found</response>
    /// <returns>
    /// A response containing the paginated list of all ride requests.
    /// If successful, the response will include the list of ride requests; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetRideRequestAllListQuery{PageNumber=pageNumber, PageSize = pageSize});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Create a new ride request
    /// </summary>
    /// <remarks>Creates a new ride request based on the provided information.
    /// This endpoint is accessible to authorized commuters only.
    /// 
    /// Sample Response 
    /// {
    ///   "success": true,
    ///   "message": "Ride request created successfully",
    ///   "value": 123
    /// }
    /// </remarks>
    /// <param name="rideRequestDto">The details for the new ride request</param>
    /// <response code="201">Indicates that the ride request was successfully created</response>
    /// <response code="400">Indicates that the ride request creation was unsuccessful due to invalid data</response>
    /// <returns>
    /// A response indicating the outcome of the ride request creation.
    /// If successful, the response will confirm the creation; otherwise, an error message will be provided.
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> Post([FromBody] CreateRideRequestDto rideRequestDto)
    {
        rideRequestDto.UserId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new CreateRideRequestCommand { RideRequestDto = rideRequestDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    /// <summary>
    /// Update a ride request
    /// </summary>
    /// <remarks>Updates an existing ride request based on the provided information.
    /// This endpoint is accessible to authorized commuters only.
    ///
    /// Sample Response 
    /// {
    ///   "success": true,
    ///   "message": "Ride request update successfully",
    ///   "value": 123
    /// }</remarks>
    /// <param name="rideRequestDto">The updated details for the ride request</param>
    /// <response code="200">Indicates that the ride request was successfully updated</response>
    /// <response code="400">Indicates that the ride request update was unsuccessful due to invalid data</response>
    /// <response code="403">Indicates that the user is not authorized to update this ride request</response>
    /// <response code="404">Indicates that the specified ride request was not found</response>
    /// <returns>
    /// A response indicating the outcome of the ride request update.
    /// If successful, the response will confirm the update; otherwise, an error message will be provided.
    /// </returns>
    [HttpPut]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> Put([FromBody] UpdateRideRequestDto rideRequestDto)
    {
        var result = await _mediator.Send(new UpdateRideRequestCommand { RideRequestDto = rideRequestDto ,UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    /// <summary>
    /// Delete a ride request by ID
    /// </summary>
    /// <remarks>Deletes a specific ride request from the system based on its unique identifier.
    /// This endpoint is accessible to administrators only.
    /// </remarks>
    /// <param name="id">The unique identifier of the ride request to delete</param>
    /// <response code="204">Indicates that the ride request was successfully deleted</response>
    /// <response code="404">Indicates that the specified ride request was not found</response>
    /// <returns>
    /// A response indicating the outcome of the delete operation.
    /// If successful, the response will confirm the deletion; otherwise, an error message will be provided.
    /// </returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideRequestCommand { Id = id });

        var status = result.Success ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Cancel a ride request by ID
    /// </summary>
    /// <remarks>Cancels a specific ride request in the system based on its unique identifier.
    /// This endpoint is accessible to authorized commuters only.
    /// </remarks>
    /// <param name="id">The unique identifier of the ride request to cancel</param>
    /// <response code="200">Indicates that the ride request was successfully canceled</response>
    /// <response code="400">Indicates that the ride request cancellation was unsuccessful due to invalid data</response>
    /// <returns>
    /// A response indicating the outcome of the ride request cancellation.
    /// If successful, the response will confirm the cancellation; otherwise, an error message will be provided.
    /// </returns>
    [HttpPut("cancel/{id}")]
    [Authorize(Roles = "Commuter")]
    public async Task<IActionResult> CancelRideRequest(int id)
    {
        var result = await _mediator.Send(new UpdateRideRequestStatusCommand{ Id = id ,UserId = _userAccessor.GetUserId()});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }
}
