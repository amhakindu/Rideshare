using System.Net;
using Rideshare.Application.Contracts.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Features.Userss;

namespace Rideshare.WebApi.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IUserAccessor _userAccessor;
    protected readonly IMediator _mediator;

    public BaseApiController(IMediator mediator, IUserAccessor userAccessor)
    {
        _mediator = mediator;
        _userAccessor = userAccessor;
    }

    public ActionResult getResponse<T>(HttpStatusCode status, T? payload){

        if(status == HttpStatusCode.Created){
            return Created("", payload);
        }else if(status == HttpStatusCode.BadRequest){
            return BadRequest(payload);
        }else if(status == HttpStatusCode.OK){
            return Ok(payload);
        }else if(status == HttpStatusCode.NotFound){
            return NotFound(payload);
        }else if(status == HttpStatusCode.Accepted){
            return Accepted(payload);
        }
        else if(status == HttpStatusCode.Unauthorized){
            return Unauthorized(payload);
        }
        return NoContent();
    }
}
