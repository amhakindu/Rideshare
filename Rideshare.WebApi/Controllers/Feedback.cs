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

namespace Rideshare.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Feedback: BaseApiController
    {
        public Feedback(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator, unitOfWork)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetFeedbackListQuery {  });
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse<BaseResponse<List<FeedbackDto>>>(status, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetFeedbackDetailQuery { Id = id });
            var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return getResponse<BaseResponse<FeedbackDto>>(status, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFeedbackDto feedbackDto)
        {
            var result = await _mediator.Send(new CreateFeedBackCommand { feedbackDto = feedbackDto });

            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse<BaseResponse<Unit>>(status, result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateFeedbackDto feedbackDto)
        {
            var result = await _mediator.Send(new UpdateFeedbackCommand { feedbackDto = feedbackDto });

            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse<BaseResponse<Unit>>(status, result);
        }

        [HttpDelete]
        public async Task<IActionResult> Put(int id)
        {
            var result = await _mediator.Send(new DeleteFeedbackCommand { Id = id });
            var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return getResponse<BaseResponse<Unit>>(status, result);
        }
    }
}
