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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] UserCreationDto userCreationDto)
    {
        var result = await _mediator.Send(new CreateUserCommand { UserCreationDto = userCreationDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<ApplicationUser>>(status, result);
    }

    [HttpPost("{id}/verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyUser(string id, [FromBody] VerifyRequest verifyRequest)
    {
        var result = await _mediator.Send(new VerifyUserCommand { UserId = id, Code = verifyRequest });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<bool>>(status, result);
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> Update(string id, [FromBody] UserUpdatingDto userUpdatingDto)
    {
        var result = await _mediator.Send(new UpdateUserCommand { UserId = id, User = userUpdatingDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<ApplicationUser>>(status, result);
    }
}
