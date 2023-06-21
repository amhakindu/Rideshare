using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.Commands;

using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : BaseApiController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {


        var result = await _mediator.Send(new LoginCommand { LoginRequest = loginRequest });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<LoginResponse>>(status, result);
    }

    [HttpPost("admin/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAdmin(LoginRequestByAdmin loginRequest)
    {


        var result = await _mediator.Send(new AdminLoginCommand { LoginRequest = loginRequest });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<LoginResponse>>(status, result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromForm] UserCreationDto userCreationDto)
    {
        var result = await _mediator.Send(new CreateUserCommand { UserCreationDto = userCreationDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<UserDto>>(status, result);
    }

    [HttpPost("/admin")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] AdminCreationDto userCreationDto)
    {
        var result = await _mediator.Send(new CreateAdminUserCommand { AdminCreationDto = userCreationDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<AdminUserDto>>(status, result);
    }



    [HttpPut("{id}")]

    public async Task<IActionResult> Update(string id, [FromForm] UserUpdatingDto userUpdatingDto)
    {
        var result = await _mediator.Send(new UpdateUserCommand { UserId = id, User = userUpdatingDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<UserDto>>(status, result);
    }

    [HttpPut("admin/{id}")]

    public async Task<IActionResult> UpdateAdmin(string id, [FromBody] AdminUpdatingDto userUpdatingDto)
    {
        var result = await _mediator.Send(new UpdateAdminUserCommand { UserId = id, UpdatingDto = userUpdatingDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<AdminUserDto>>(status, result);
    }
}

