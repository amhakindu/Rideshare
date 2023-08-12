using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Features.Auth.Queries;
using Rideshare.Application.Features.Auth.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : BaseApiController
{
	public UserController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
	{
	}
	
	/// <summary>
	/// Authenticate and log in a user.
	/// </summary>
	/// <remarks>Authenticates the user's credentials and returns a JWT access and refresh tokens if successful.
	/// 
	///Sample Response for successful authentication:
	/// {
	///	 "success": true,
	///	 "message": "Logged In Successfully",
	///	 "value": {
	///		"message": "Login successful",
	///		"accessToken": "eyJhb...xyz",
	///		"refreshToken": "NPkJ...zyx"
	///  },
	///	 "errors": []
	/// }
	/// </remarks>
	[HttpPost("login")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
	public async Task<IActionResult> Login(LoginRequest loginRequest)
	{
		var result = await _mediator.Send(new LoginCommand { LoginRequest = loginRequest });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse<BaseResponse<LoginResponse>>(status, result);
	}
	
	/// <summary>
	/// Authenticate and log in an admin user.
	/// </summary>
	/// <remarks>Authenticates the admin's credentials and returns access and refresh JWT tokens if it's successful.
	/// 
	///Sample Response:
	/// {
	///	"success": true,
	///	"message": "Logged In Successfully",
	///	"value": {
	///		"message": "Login successful",
	///		"accessToken": "eyJhb...xyz",
	///		"refreshToken": "NPkJ...zyx"
	///	},
	///	"errors": []
	/// }
	/// </remarks>

	[HttpPost("admin/login")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
	public async Task<IActionResult> LoginAdmin(LoginRequestByAdmin loginRequest)
	{


		var result = await _mediator.Send(new AdminLoginCommand { LoginRequest = loginRequest });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse<BaseResponse<LoginResponse>>(status, result);
	}

	/// <summary>
	/// Create a new user.
	/// </summary>
	/// <remarks>Registers a new user with the given details. The user is not an admin or driver by default.
	/// 
	/// Sample Response for successful registration:
	/// {
	///    "success": true,
	///	"message": "Commuter Created Successfully",
	///	"value": {
	///		"roles": [],
	///		"fullName": "Kasahun Ayele",
	///		"phoneNumber": "+251902459334",
	///		"age": 12,
	///		"statusByLogin": null,
	///		"profilePicture": "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1691653061/cmeqykjados9afpiddl8.png"
	///	},
	///	"errors": []
	/// }
	/// </remarks>
	/// <response code="201">User successfully created.</response>
	/// <response code="400">Failed to create user due to invalid data.</response>
	[HttpPost]
	[AllowAnonymous]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
	public async Task<IActionResult> Post([FromForm] UserCreationDto userCreationDto)
	{
		var result = await _mediator.Send(new CreateUserCommand { UserCreationDto = userCreationDto });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse<BaseResponse<UserDto>>(status, result);
	}

	/// <summary>
	/// Create a new admin user.
	/// </summary>
	/// <remarks> Registers a new user with administrative privileges based on the provided details.
	///
	/// Sample Response for successful creation:
	///  {
	///    "success": true,
	///	   "message": "Admin Created Successfully",
	///	   "value": {
	///	      "roles": [],
	///	   	  "userName": "Linger",
	///	   	  "fullName": "Lingerew Melakun",
	///	   	  "password": "",
	///	   	  "email": "lingerew@gmail.com",
	///	   	  "phoneNumber": "+251907902541",
	///	   	  "age": 12
	///	   },
	///	   "errors": []
	/// }
	/// </remarks>
	[HttpPost("admin")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(AdminUserDto), StatusCodes.Status201Created)]
	public async Task<IActionResult> CreateUser([FromForm] AdminCreationDto userCreationDto)
	{
		var result = await _mediator.Send(new CreateAdminUserCommand { AdminCreationDto = userCreationDto });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse<BaseResponse<AdminUserDto>>(status, result);
	}
	
	/// <summary>
	/// Create a new driver user.
	/// </summary>
	/// <remarks>Registers a new user with driver privileges based on the provided details. 
	/// This endpoint allows for easy onboarding of drivers for the ride-share platform.
	///
	/// Sample Response for successful driver registration:
	/// {
	///  "success": true,
	///  "message": "Driver Created Successfully",
	///  "value": {
	///  	"fullName": "Gizaw Dagne",
	///  	"phoneNumber": "+251944449445",
	///  	"age": 34,
	///  	"statusByLogin": null,
	///  	"driverId": 20,
	///  	"userId": "a4061951-7135-485d-8a6a-f599f1a7d2fb",
	///  	"profilePicture": ""
	///  },
	///  "errors": []
	/// </remarks>
	/// <response code="201">Driver user successfully created.</response>
	/// <response code="400">Failed to create driver user due to invalid data or server error.</response>
	[HttpPost("driver")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(UserDriverDto), StatusCodes.Status201Created)]
	public async Task<IActionResult> CreateDriverUser([FromForm] DriverCreatingDto userCreationDto)
	{
		var result = await _mediator.Send(new CreateDriverCommand { DriverCreatingDto = userCreationDto });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse<BaseResponse<UserDriverDto>>(status, result);
	}

	/// <summary>
	/// Update a user's details.
	/// </summary>
	/// <remarks>Allows modification of an existing user's details based on the provided user ID and updated information. 
	/// This endpoint is useful for updating user profiles and correcting any inaccuracies in their details.</remarks>
	/// <response code="200">User details successfully updated.</response>
	/// <response code="400">Failed to update user details due to invalid data or server error.</response>
	[HttpPut("{id}")]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(string id, [FromForm] UserUpdatingDto userUpdatingDto)
	{
		var result = await _mediator.Send(new UpdateUserCommand { UserId = id, User = userUpdatingDto });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse<BaseResponse<UserDto>>(status, result);
	}

	/// <summary>
	/// Update an admin user's details.
	/// </summary>
	/// <remarks>Allows modification of an existing admin user's details based on the provided admin user ID and updated informations. 
	/// </remarks>
	/// <response code="200">Admin user details successfully updated.</response>
	/// <response code="400">Failed to update admin user details due to invalid data or server error.</response>
	[HttpPut("admin/{id}")]
	[ProducesResponseType(typeof(AdminUserDto), StatusCodes.Status200OK)]
	public async Task<IActionResult> UpdateAdmin(string id, [FromBody] AdminUpdatingDto userUpdatingDto)
	{
		var result = await _mediator.Send(new UpdateAdminUserCommand { UserId = id, UpdatingDto = userUpdatingDto });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse<BaseResponse<AdminUserDto>>(status, result);
	}

	/// <summary>
	/// Retrieve details of the currently authenticated user.
	/// </summary>
	/// <remarks> Fetches and returns the details of the currently authenticated user. This endpoint aids in tasks such as profile viewing and user settings adjustments for the authenticated user.
	///
	/// Sample Response:
	/// {
	///	"success": true,
	///	"message": "Fetched In Successfully",
	///	"value": {
	///		"roles": [
	///		{
	///			"id": "3c5eafb2-7438-421d-b0a2-f6ad4ff4bce6",
	///			"name": "Admin"
	///		}
	///		],
	///		"fullName": "Admin",
	///		"phoneNumber": "+2519393423022",
	///		"age": 0,
	///		"statusByLogin": "ACTIVE",
	///		"profilePicture": ""
	///	},
	///	"errors": []
	/// </remarks>
	/// <response code="200">Successfully retrieved the authenticated user's details.</response>
	/// <response code="400">Failed to retrieve the user details due to server error or unauthorized request.</response>
	[HttpGet()]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]

	public async Task<IActionResult> GetUserId()
	{
		var result = await _mediator.Send(new GetUserByIdQuery { UserId = _userAccessor.GetUserId() });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse<BaseResponse<UserDto>>(status, result);
	}

	/// <summary>
	/// Retrieve the details of a user based on their ID.
	/// </summary>
	/// <remarks>Fetches and returns the details of the user corresponding to the given unique identifier. 
	/// This endpoint is useful for detailed views of specific users or administrative purposes.
	///
	/// Sample Response:
	///	"success": true,
	///	"message": "Fetched In Successfully",
	///	"value": {
	///		"roles": [
	///		{
	///			"id": "eed374a7-da83-4eb5-8bf5-d524434194a2",
	///			"name": "Commuter"
	///		}
	///		],
	///		"fullName": "Gizaw Dagne",
	///		"phoneNumber": "+251944444445",
	///		"age": 0,
	///		"statusByLogin": "INACTIVE",
	///		"profilePicture": ""
	///	},
	///	"errors": []
	/// </remarks>
	/// <response code="200">Successfully retrieved the user's details.</response>
	/// <response code="400">Failed to retrieve the user details due to server error or invalid request.</response>
	[HttpGet("withAGiven/{id}")]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]

	public async Task<IActionResult> GetUserById(string id)
	{
		var result = await _mediator.Send(new GetUserByIdQuery { UserId = id });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse<BaseResponse<UserDto>>(status, result);

	}
	
	/// <summary>
	/// Retrieve a paginated list of all users.
	/// </summary>
	/// <remarks>Fetches and returns a paginated list of all users. The number of users returned per page and the page number can be specified. 
	/// This is especially handy for administrative views or user management screens where pagination is necessary.
	///
	/// Sample Response:
	/// {
	///	"pageNumber": 1,
	///	"pageSize": 10,
	///	"count": 1000,
	///	"success": true,
	///	"message": "Users Fetched Successfully",
	///	"value": [
	///		{
	///		"roles": [
	///			{
	///			"id": "086c323c-c844-4843-9a0d-133ad7af0aa4",
	///			"name": "Commuter"
	///			}
	///		   ],
	///		"id": "01a2b34c-5d6e-4890-1a2b-123456789033",
	///		"fullName": "Hana Solomon",
	///		"phoneNumber": "+251911111133",
	///		"age": 0,
	///		"statusByLogin": "INACTIVE",
	///		"profilePicture": "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
	///		},
	///		...9 other users
	///	   ]
	/// }
	/// </remarks>
	/// <response code="200">Successfully retrieved the list of users.</response>
	/// <response code="404">No users found or server error occurred.</response>
	[HttpGet("all")]
	[ProducesResponseType(typeof(PaginatedResponse<UserDtoForAdmin>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAllUser([FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Retrieve a paginated list of users based on a specific role.
	/// </summary>
	/// <remarks>Fetches and returns a paginated list of users corresponding to the given role. 
	/// 
	///sample response for successful fetch of users with a role:
	///{
	///	 "success": true,
	///	 "message": "Roles Fetching Successful",
	///	 "value": [
	///		{
	///		"id": "bcaa5c92-d9d8-4106-8150-91cb40139030",
	///		"name": "Admin"
	///		},
	///		other roles...
	///	 ],
	///	 "errors": []
	/// }</remarks>
	/// <response code="200">Successfully retrieved the list of users.</response>
	/// <response code="404">No users found for the specified role or server error occurred.</response>
	[HttpGet("by-role")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(typeof(PaginatedResponse<UserDtoForAdmin>), StatusCodes.Status200OK)]

	public async Task<IActionResult> GetUsersByRole([FromQuery] string role, [FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 10)
	{


		var result = await _mediator.Send(new GetUsersByRoleQuery { Role = role, PageNumber = pageNumber, PageSize = pageSize });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}

	/// <summary>
	/// Retrieve a list of all roles.
	/// </summary>
	/// <remarks>Fetches and returns a list of all roles in the system. This endpoint is useful for populating role dropdowns or for administrative purposes.
	/// 
	///sample response for successful fetch of roles:
	///{
	///	 "success": true,
	///	 "message": "Roles Fetching Successful",
	///	 "value": [
	///		{
	///		"id": "bcaa5c92-d9d8-4106-8150-91cb40139030",
	///		"name": "Admin"
	///		},
	///		other roles...
	///	 ],
	///	 "errors": []
	/// }</remarks>
	/// <response code="200">Successfully retrieved the list of roles.</response>
	/// <response code="404">No roles found or server error occurred.</response>
	[HttpGet("roles/all")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAllRoles()
	{


		var result = await _mediator.Send(new GetAllRolesQuery { });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Retrieve a paginated list of users based on specific filter criteria.
	/// </summary>
	/// <remarks>Fetches and returns a paginated list of users based on filter parameters such as phone number, role name, full name, and status.
	/// This is particularly useful when implementing advanced search functionality in the system.
	///
	///sample response:
	///{
	///	 "pageNumber": 1,
	///	 "pageSize": 10,
	///	 "count": 10,
	///	 "success": true,
	///	 "message": "Users Fetched Successfully",
	///	 "value": [
	///		{
	///		"roles": [
	///			{
	///			"id": "6d3d3723-072b-4558-b1a5-448d4219bdbb",
	///			"name": "Commuter"
	///			}
	///		],
	///		"id": "012d3aef-4b56-471c-86d9-123456789004",
	///		"fullName": "Bruk Assefa",
	///		"phoneNumber": "+251911111105",
	///		"age": 0,
	///		"statusByLogin": "INACTIVE",
	///		"profilePicture": ""
	///		},
	///		9 other users...
	///	 ],
	///	 "errors": []
	///	}
	/// </remarks>
	/// <response code="200">Successfully retrieved the list of users based on filter criteria.</response>
	/// <response code="400">Bad request or server error occurred.</response>
	[HttpGet("filter")]
	[ProducesResponseType(typeof(PaginatedResponse<UserDtoForAdmin>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetUsersByFilter(
		   [FromQuery] string? phoneNumber,
		   [FromQuery] string? roleName,
		   [FromQuery] string? fullName,
		   [FromQuery] string? status,
		   [FromQuery] int pageNumber = 1,
		   [FromQuery] int pageSize = 10)
	{
		var query = new GetUsersByFilterQuery
		{
			PhoneNumber = phoneNumber ?? "",
			RoleName = roleName ?? "",
			FullName = fullName ?? "",
			Status = status ?? "",
			PageNumber = pageNumber,
			PageSize = pageSize
		};

		var result = await _mediator.Send(query);

		var statuss = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
		return getResponse<PaginatedResponse<UserDtoForAdmin>>(statuss, result);
	}
	
	/// <summary>
	/// Deletes a user based on their ID.
	/// </summary>
	/// <remarks>Permanently removes a user from the system based on their unique identifier.
	/// </remarks>
	/// <response code="200">Successfully deleted the user.</response>
	/// <response code="404">User not found or server error occurred.</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(Double), StatusCodes.Status200OK)]

	public async Task<IActionResult> Delete(string id)
	{
		var result = await _mediator.Send(new DeleteUserCommand { UserId = id });

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse<BaseResponse<Double>>(status, result);
	}
}

