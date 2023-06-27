using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Commands;
using Rideshare.Application.Features.Feedbacks.Queries;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
using System.Net;
using System.Security.Claims;

namespace Rideshare.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Feedback: BaseApiController
    {
        public Feedback(IMediator mediator, IUserAccessor userAccessor) : base(mediator, userAccessor)
        {
        }

        [HttpGet]
        // only admin can view
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetFeedbackListQuery { PageNumber = pageNumber, PageSize = pageSize });
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpGet("{id}")]
        // only admin can view
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetFeedbackDetailQuery { Id = id });
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpGet("byUserId")]
        // only admin can view
        [Authorize(Roles = "Commuter,Admin")]
        public async Task<IActionResult> GetByUserId([FromQuery] string userId, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
        {
            var result = await _mediator.Send(new GetFeedbackListByUserIdQuery { UserId = userId, PageNumber = PageNumber, PageSize=PageSize });
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse(status, result);
        }

        [HttpPost]
        // only driver and commuter can post
        [Authorize(Roles = "Commuter,Driver" )]
        public async Task<IActionResult> Post([FromBody] CreateFeedbackDto feedbackDto)
        {
            var result = await _mediator.Send(new CreateFeedBackCommand { feedbackDto = feedbackDto });

            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse(status, result);
        }

        [HttpPut]
        [Authorize(Roles = "Commuter,Driver")]
        // only creater can update
        public async Task<IActionResult> Put([FromBody] UpdateFeedbackDto feedbackDto)
        {
            var status = HttpStatusCode.OK;

            BaseResponse<FeedbackDto> response = await _mediator.Send(new GetFeedbackDetailQuery { Id = feedbackDto.Id });
            FeedbackDto? feedback = response.Value;
            string? currentUser = _userAccessor.GetUserId();
            //throw new Exception(currentUser);
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

        [HttpDelete]
        [Authorize(Roles = "Commuter,Driver")]
        public async Task<IActionResult> Delete(int id)
        {
            // get feedback check the current feedback user id == current user id
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
}
