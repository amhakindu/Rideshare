using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Commands;
using Rideshare.Application.Features.Feedbacks.Queries;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Responses;
using System.Net;
using System.Security.Claims;

namespace Rideshare.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Feedback: BaseApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Feedback(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        // only admin can view
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetFeedbackListQuery {  });
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

        [HttpPost]
        // only driver and commuter can post
        public async Task<IActionResult> Post([FromBody] CreateFeedbackDto feedbackDto)
        {
            var result = await _mediator.Send(new CreateFeedBackCommand { feedbackDto = feedbackDto });

            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse(status, result);
        }

        [HttpPut]
        // only creater can update
        public async Task<IActionResult> Put([FromBody] UpdateFeedbackDto feedbackDto)
        {
            var result = await _mediator.Send(new UpdateFeedbackCommand { feedbackDto = feedbackDto });

            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse(status, result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            // get feedback check the current feedback user id == current user id
            string currentUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new DeleteFeedbackCommand { Id = id });
            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse(status, result);
        }

    }
}
