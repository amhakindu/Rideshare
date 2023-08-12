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

    /// <summary>
    /// Create a new ride offer
    /// </summary>
    /// <remarks>Creates a new ride offer based on the provided information.
    /// This endpoint is accessible to authorized drivers only.
    /// </remarks>
    /// <param name="createRideOfferDto">The details for the new ride offer</param>
    /// <response code="201">Indicates that the ride offer was successfully created</response>
    /// <response code="400">Indicates that the ride offer creation was unsuccessful due to invalid data</response>
    /// <response code="404">Indicates that a required resource for the ride offer creation was not found</response>
    /// <returns>
    /// A response indicating the outcome of the ride offer creation.
    /// If successful, the response will confirm the creation; otherwise, an error message will be provided.
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Driver")]
    [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateRideOfferDto createRideOfferDto)
    {       
        var result = await _mediator.Send(new CreateRideOfferCommand { RideOfferDto = createRideOfferDto, UserId = _userAccessor.GetUserId() });
        
        var status = result.Success ? HttpStatusCode.Created: HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<int>>(status, result);
    }

    /// <summary>
    /// Retrieve information about a ride offer by ID
    /// </summary>
    /// <remarks>Retrieves detailed information about a ride offer based on the associated ID.
    /// This endpoint is accessible to users with the "Driver" or "Admin" roles.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offer details retrieved successfully",
    ///     "value": {
    ///         "id": 1,
    ///         "driver": {
    ///             "id": 2,
    ///             "userId": "driver123",
    ///             "user": {
    ///                 "roles": [],
    ///                 "fullName": "John Doe",
    ///                 "phoneNumber": "1234567890",
    ///                 "age": 30,
    ///                 "statusByLogin": "active",
    ///                 "profilePicture": "http://example.com/profile.jpg"
    ///             },
    ///             "rate": [4, 5, 3],
    ///             "experience": 5.5,
    ///             "address": "123 Main St",
    ///             "licenseNumber": "ABC123",
    ///             "license": "http://example.com/license.pdf",
    ///             "verified": true
    ///         },
    ///         "vehicle": {
    ///             "id": 3,
    ///             "plateNumber": "XYZ123",
    ///             "model": "Honda Civic",
    ///             "numberOfSeats": 4,
    ///             "address": "Downtown",
    ///             "libre": "http://example.com/libre.pdf"
    ///         },
    ///         "currentLocation": {
    ///             "latitude": 37.1234,
    ///             "longitude": -122.5678
    ///         },
    ///         "destination": {
    ///             "latitude": 37.5678,
    ///             "longitude": -122.1234
    ///         },
    ///         "originAddress": "City Center",
    ///         "destinationAddress": "Suburbia",
    ///         "status": "Active",
    ///         "availableSeats": 3,
    ///         "estimatedFare": 10.0,
    ///         "estimatedDuration": "03:30:00"
    ///     },
    ///     "errors": []
    /// }
    /// </remarks>
    /// <param name="id">The unique identifier of the ride offer to retrieve</param>
    /// <response code="200">Indicates a successful retrieval of the ride offer information</response>
    /// <response code="404">Indicates that the specified ride offer was not found</response>
    /// <returns>
    /// A response containing detailed information about the requested ride offer.
    /// If successful, the response will include the ride offer's details; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Driver,Admin")]
    [ProducesResponseType(typeof(BaseResponse<RideOfferDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRideOfferWithDetailsQuery { RideOfferID = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<RideOfferDto>>(status, result);
    }
    

    /// <summary>
    /// Retrieve ride offers of the authenticated driver
    /// </summary>
    /// <remarks>Retrieves a paginated list of ride offers associated with the currently authenticated driver.
    /// This endpoint is accessible to users with the "Driver" role.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offers retrieved successfully",
    ///     "value": [
    ///             {
    ///                 "id": 1,
    ///                 "driver": {
    ///                     "id": 2,
    ///                     ...
    ///                 },
    ///                 "originAddress": "City Center",
    ///                 "destinationAddress": "Suburbia",
    ///                 "status": "Active",
    ///                 "availableSeats": 3
    ///             },
    ///             ...
    ///     ],
    ///     "pageNumber": 1,
    ///     "pageSize": 10,
    ///     "count": 20,
    ///     "errors": []
    /// }
    /// </remarks>
    /// <param name="PageNumber">The page number for pagination (default: 1)</param>
    /// <param name="PageSize">The number of items per page (default: 10)</param>
    /// <response code="200">Indicates a successful retrieval of the ride offers</response>
    /// <response code="403">Indicates that the user is not authorized to access ride offers</response>
    /// <returns>
    /// A response containing a paginated list of ride offers associated with the authenticated driver.
    /// If successful, the response will include the list of ride offers; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet]
    [Authorize(Roles = "Driver")]
    [ProducesResponseType(typeof(PaginatedResponse<RideOfferListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersOfLoggedInDriver([FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetRideOffersOfDriverQuery { Id = _userAccessor.GetUserId(), PageNumber = PageNumber, PageSize = PageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Retrieve ride offers of a specific driver by ID
    /// </summary>
    /// <remarks>Retrieves a paginated list of ride offers associated with a driver based on the specified driver ID.
    /// This endpoint is accessible to users with the "Admin" role.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offers retrieved successfully",
    ///     "value": [
    ///             {
    ///                 "id": 1,
    ///                 "driver": {
    ///                     "id": 2,
    ///                     ...
    ///                 },
    ///                 "originAddress": "City Center",
    ///                 "destinationAddress": "Suburbia",
    ///                 "status": "Active",
    ///                 "availableSeats": 3
    ///             },
    ///             ...
    ///     ],
    ///     "pageNumber": 1,
    ///     "pageSize": 10,
    ///     "count": 20,
    ///     "errors": []
    /// }</remarks>
    /// <param name="driverId">The ID of the driver for whom to retrieve ride offers</param>
    /// <param name="PageNumber">The page number for pagination (default: 1)</param>
    /// <param name="PageSize">The number of items per page (default: 10)</param>
    /// <response code="200">Indicates a successful retrieval of the ride offers</response>
    /// <response code="403">Indicates that the user is not authorized to access ride offers</response>
    /// <returns>
    /// A response containing pagination information for the paginated list of ride offers associated with the specified driver.
    /// If successful, the response will include pagination details; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("of-driver/{driverId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResponse<RideOfferListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersOfDriverWithId(int driverId, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetRideOffersOfDriverQuery { Id = driverId, PageNumber = PageNumber, PageSize = PageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    
    /// <summary>
	/// Gets all of the rideoffers
	/// </summary>
	/// <remarks> Retrieves a paginated list of all of the rideoffers created through the rideshare platform
    /// This endpoint is accessible to Admins only.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offers retrieved successfully",
    ///     "value": [
    ///             {
    ///                 "id": 1,
    ///                 "driver": {
    ///                     "id": 2,
    ///                     ...
    ///                 },
    ///                 "originAddress": "City Center",
    ///                 "destinationAddress": "Suburbia",
    ///                 "status": "Active",
    ///                 "availableSeats": 3
    ///             },
    ///             ...
    ///     ],
    ///     "pageNumber": 1,
    ///     "pageSize": 10,
    ///     "count": 20,
    ///     "errors": []
    /// }</remarks> 
	/// <response code="200">RideOffers fetching successfull</response>
	/// <returns>
	/// A list of rideoffers
	/// </returns>
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResponse<RideOfferListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetAllRideOffersQuery(){ PageNumber= PageNumber, PageSize= PageSize});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<PaginatedResponse<RideOfferListDto>>(status, result);
    }

    /// <summary>
    /// Search and filter ride offers
    /// </summary>
    /// <remarks>Retrieves a paginated list of ride offers based on search criteria.
    /// This endpoint is accessible to Admins only.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offers retrieved successfully",
    ///     "value": [
    ///             {
    ///                 "id": 1,
    ///                 "driver": {
    ///                     "id": 2,
    ///                     ...
    ///                 },
    ///                 "originAddress": "City Center",
    ///                 "destinationAddress": "Suburbia",
    ///                 "status": "Active",
    ///                 "availableSeats": 3
    ///             },
    ///             ...
    ///     ],
    ///     "pageNumber": 1,
    ///     "pageSize": 10,
    ///     "count": 20,
    ///     "errors": []
    /// }</remarks>
    /// <param name="SearchDto">Search filter parameters</param>
    /// <param name="PageNumber">Page number (default: 1)</param>
    /// <param name="PageSize">Number of items per page (default: 10)</param>
    /// <response code="200">Rideoffers fetching successful</response>
    /// <response code="406">Invalid search filter parameters </response>
    /// <returns>A list of ride offers</returns>
    [HttpGet("filter")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SearchAndFilter([FromQuery] RideOffersListFilterDto SearchDto, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new SearchAndFilterQuery { PageNumber = PageNumber, PageSize = PageSize, SearchDto = SearchDto });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Update a ride offer
    /// </summary>
    /// <remarks>Updates the details of a ride offer using the provided data.
    /// This endpoint is accessible to users with the "Driver" or "Admin" roles.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offer updated successfully",
    ///     "errors": []
    /// }
    /// </remarks>
    /// <param name="updateRideOfferDto">The data to update the ride offer</param>
    /// <response code="202">Indicates that the update has been accepted</response>
    /// <response code="403">Indicates that the user is not authorized to update ride offers</response>
    /// <response code="404">Indicates that the ride offer to be updated was not found</response>
    /// <response code="406">Indicates that the provided data is not acceptable for the update</response>
    /// <returns>
    /// A response indicating the outcome of the ride offer update.
    /// If successful, the response will confirm the update; otherwise, an error message will be provided.
    /// </returns>
    [HttpPatch]  
    [Authorize(Roles = "Driver")]
    [ProducesResponseType(typeof(BaseResponse<Unit>), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Patch([FromBody] UpdateRideOfferDto updateRideOfferDto){
        var result = await _mediator.Send(new UpdateRideOfferCommand { RideOfferDto = updateRideOfferDto });

        var status = result.Success ? HttpStatusCode.Accepted : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<Unit>>(status, result);
    }

    /// <summary>
    /// Delete a ride offer
    /// </summary>
    /// <remarks>Deletes a ride offer with the specified ID.
    /// This endpoint is accessible to users with the "Admin" role.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offer deleted successfully",
    ///     "errors": []
    /// }
    /// </remarks>
    /// <param name="id">The ID of the ride offer to be deleted</param>
    /// <response code="200">Indicates a successful deletion of the ride offer</response>
    /// <response code="404">Indicates that the ride offer to be deleted was not found</response>
    /// <returns>
    /// A response indicating the outcome of the ride offer deletion.
    /// If successful, the response will confirm the deletion; otherwise, an error message will be provided.
    /// </returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BaseResponse<Unit>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRideOfferCommand { RideOfferID = id });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Unit>>(status, result);
    }
    
    /// <summary>
    /// Cancel a ride offer
    /// </summary>
    /// <remarks>Cancels a ride offer with the specified ID.
    /// This endpoint is accessible to users with the "Driver" role.
    ///
    /// Sample Response:
    /// {
    ///     "success": true,
    ///     "message": "Ride offer canceled successfully",
    ///     "errors": []
    /// }
    /// </remarks>
    /// <param name="id">The ID of the ride offer to be canceled</param>
    /// <response code="200">Indicates a successful cancellation of the ride offer</response>
    /// <response code="403">Indicates that the user is not authorized to cancel this ride offer</response>
    /// <response code="404">Indicates that the ride offer to be canceled was not found</response>
    /// <returns>
    /// A response indicating the outcome of the ride offer cancellation.
    /// If successful, the response will confirm the cancellation; otherwise, an error message will be provided.
    /// </returns>
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
