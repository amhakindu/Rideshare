using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Features.Feedbacks.Queries;
using Rideshare.Application.Features.Feedbacks.Commands;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/feedbacks")]
public class Feedback: BaseApiController
{
    public Feedback(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetFeedbackListQuery { PageNumber = pageNumber, PageSize = pageSize });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetFeedbackDetailQuery { Id = id });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    // [HttpGet]
    // [Authorize(Roles = "Commuter")]
    // public async Task<IActionResult> GetFeedbacksOfCurrentUser([FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    // {
    //     var result = await _mediator.Send(new GetFeedbackListByUserIdQuery { UserId = _userAccessor.GetUserId(), PageNumber = PageNumber, PageSize=PageSize });
    //     var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
    //     return getResponse(status, result);
    // }

    [HttpGet("of-user/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetFeedbacksOfUser(string userId, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
    {
        var result = await _mediator.Send(new GetFeedbackListByUserIdQuery { UserId = userId, PageNumber = PageNumber, PageSize=PageSize });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    [HttpPost]
    [Authorize(Roles = "Commuter,Driver" )]
    public async Task<IActionResult> Post([FromBody] CreateFeedbackDto feedbackDto)
    {
        var result = await _mediator.Send(new CreateFeedBackCommand { feedbackDto = feedbackDto });

        var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        return getResponse(status, result);
    }

    [HttpPut]
    [Authorize(Roles = "Commuter,Driver")]
    public async Task<IActionResult> Put([FromBody] UpdateFeedbackDto feedbackDto)
    {
        var status = HttpStatusCode.OK;

        BaseResponse<FeedbackDto> response = await _mediator.Send(new GetFeedbackDetailQuery { Id = feedbackDto.Id });
        FeedbackDto? feedback = response.Value;
        string? currentUser = _userAccessor.GetUserId();
        var result = new BaseResponse<int>
        {
            Success = false,
            Message = "Unsuccesful"
        };
        if (feedback == null)
            status = HttpStatusCode.BadRequest;
        else if (feedback.UserId != currentUser)
            status = HttpStatusCode.Unauthorized;
        else
        {
            result = await _mediator.Send(new UpdateFeedbackCommand { feedbackDto = feedbackDto });
            if (!result.Success)
                status = HttpStatusCode.BadRequest;
        }
        return getResponse(status, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Commuter,Driver")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = HttpStatusCode.OK;
        BaseResponse<FeedbackDto> response = await _mediator.Send(new GetFeedbackDetailQuery { Id = id });
        FeedbackDto? feedback = response.Value;
        string? userId = _userAccessor.GetUserId();
        var result = new BaseResponse<int>
        {
            Success = false,
            Message = "Unsuccesful"
        };
        
        if (feedback == null)
            status = HttpStatusCode.BadRequest;
        else if (feedback.UserId != userId)
            status = HttpStatusCode.Unauthorized;
        else
        {
            result = await _mediator.Send(new DeleteFeedbackCommand { Id = id });
            if (!result.Success)
                status = HttpStatusCode.BadRequest;
        }
        return getResponse(status, result);
    }
}