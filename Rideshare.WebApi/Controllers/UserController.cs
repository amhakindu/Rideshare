using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Features.Auth.Queries;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : BaseApiController
{

    public UserController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {


        var result = await _mediator.Send(new LoginCommand { LoginRequest = loginRequest });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<LoginResponse>>(status, result);
    }

    [HttpPost("admin/login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAdmin(LoginRequestByAdmin loginRequest)
    {


        var result = await _mediator.Send(new AdminLoginCommand { LoginRequest = loginRequest });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<LoginResponse>>(status, result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromForm] UserCreationDto userCreationDto)
    {
        var result = await _mediator.Send(new CreateUserCommand { UserCreationDto = userCreationDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<UserDto>>(status, result);
    }

    [HttpPost("admin")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AdminUserDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser([FromForm] AdminCreationDto userCreationDto)
    {
        var result = await _mediator.Send(new CreateAdminUserCommand { AdminCreationDto = userCreationDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<AdminUserDto>>(status, result);
    }

    [HttpPost("driver")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDriverDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDriverUser([FromForm] DriverCreatingDto userCreationDto)
    {
        var result = await _mediator.Send(new CreateDriverCommand { DriverCreatingDto = userCreationDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<UserDriverDto>>(status, result);
    }



    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string id, [FromForm] UserUpdatingDto userUpdatingDto)
    {
        var result = await _mediator.Send(new UpdateUserCommand { UserId = id, User = userUpdatingDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<UserDto>>(status, result);
    }

    [HttpPut("admin/{id}")]
    [ProducesResponseType(typeof(AdminUserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAdmin(string id, [FromBody] AdminUpdatingDto userUpdatingDto)
    {
        var result = await _mediator.Send(new UpdateAdminUserCommand { UserId = id, UpdatingDto = userUpdatingDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<AdminUserDto>>(status, result);
    }

    [HttpGet()]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetUserId()
    {
        var result = await _mediator.Send(new GetUserByIdQuery { UserId = _userAccessor.GetUserId() });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<UserDto>>(status, result);
    }

    [HttpGet("withAGiven/{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetUserById(string id)
    {
        Console.WriteLine(id);
        Console.WriteLine("cha");
        var result = await _mediator.Send(new GetUserByIdQuery { UserId = id });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<UserDto>>(status, result);

    }
    [HttpGet("Top5Commuter")]
    [ProducesResponseType(typeof( List<CommuterWithRideRequestCntDto>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetTop5Commuter()
    {
        var result = await _mediator.Send(new GetTop5CommuterQuery { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(PaginatedUserList), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetAllUser([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {


        var result = await _mediator.Send(new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    [HttpGet("by-role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedUserList), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetUsersByRole([FromQuery] string role, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {


        var result = await _mediator.Send(new GetUsersByRoleQuery { Role = role, PageNumber = pageNumber, PageSize = pageSize });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("roles/all")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles()
    {


        var result = await _mediator.Send(new GetAllRolesQuery { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("filter")]
    [ProducesResponseType(typeof(PaginatedUserList), StatusCodes.Status200OK)]
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
            PhoneNumber = phoneNumber,
            RoleName = roleName,
            FullName = fullName,
            Status = status,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        var statuss = result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<PaginatedUserList>>(statuss, result);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Double), StatusCodes.Status200OK)]

    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { UserId = id });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Double>>(status, result);
    }
     [HttpGet("statstics/week")]
     [Authorize(Roles ="Admin")]
     [ProducesResponseType(typeof(CommuterCountDto), StatusCodes.Status200OK)]

    public async Task<IActionResult> Handle()
    {
        var result = await _mediator.Send(new GetCommuterCountQuery {  });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<CommuterCountDto>>(status, result);
    }
}

