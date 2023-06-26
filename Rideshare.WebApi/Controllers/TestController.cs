using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Responses;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestController : BaseApiController
{
    public TestController(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetTestEntityQuery { TestEntityID = id });
        
        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<TestEntityDto>>(status, result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TestEntityDto createTestEntity)
    {
        var result = await _mediator.Send(new CreateTestEntityCommand { TestDto = createTestEntity });
        
        var status = result.Success ? HttpStatusCode.Created: HttpStatusCode.BadRequest;
        return getResponse<BaseResponse<int>>(status, result);
    }
}
