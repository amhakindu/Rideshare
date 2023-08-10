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

	/// <summary>
	/// Get a paginated list of all feedbacks.
	/// </summary>
	/// <remarks>Returns a paginated list of all feedbacks, accessible to "Admin" users.
	/// Provides efficient navigation through available feedbacks with details such as Id, UserId, Rating, Comments, and more.
	/// </remarks>
	/// <response code="200">Returns the paginated list of feedbacks.</response>
	/// <returns>A paginated collection of all feedbacks.</returns>
	[HttpGet("all")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediator.Send(new GetFeedbackListQuery { PageNumber = pageNumber, PageSize = pageSize });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	/// <summary>
	/// Get details of a specific feedback.
	/// </summary>
	/// <remarks>Retrieves details of a specific feedback based on its unique identifier.
	/// This endpoint is accessible to "Admin" users.
	/// </remarks>
	/// <response code="200">Returns the details of the specified feedback.</response>
	/// <response code="404">Feedback not found.</response>
	/// <returns>Details of the specified feedback.</returns>
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


	/// <summary>
	/// Get feedbacks of a specific user.
	/// </summary>
	/// <remarks>Retrieves a paginated list of feedbacks given by a user based on their unique identifier.
	/// This endpoint is accessible to "Admin" users.
	/// </remarks>
	/// <response code="200">Returns the paginated list of feedbacks.</response>
	/// <returns>A paginated collection of feedbacks given by the specified user.</returns>
	[HttpGet("of-user/{userId}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetFeedbacksOfUser(string userId, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=10)
	{
		var result = await _mediator.Send(new GetFeedbackListByUserIdQuery { UserId = userId, PageNumber = PageNumber, PageSize=PageSize });
		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Post a new feedback.
	/// </summary>
	/// <remarks>Posts a new feedback.
	/// This endpoint is accessible to users with the "Commuter" and "Driver" roles.
	/// </remarks>
	/// <returns>Details of the newly posted feedback.</returns>
	[HttpPost]
	[Authorize(Roles = "Commuter,Driver" )]
	public async Task<IActionResult> Post([FromBody] CreateFeedbackDto feedbackDto)
	{
		var result = await _mediator.Send(new CreateFeedBackCommand { feedbackDto = feedbackDto });

		var status = result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
		return getResponse(status, result);
	}
	
	/// <summary>
	/// Update an existing feedback.
	/// </summary>
	/// <remarks>Updates an existing feedback given by the user.
	/// This endpoint is accessible to users with the "Commuter" and "Driver" roles.
	/// </remarks>
	/// <returns>A message indicating the result of the update operation.</returns>
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
	
	/// <summary>
	/// Delete a feedback.
	/// </summary>
	/// <remarks>Deletes a specific feedback based on its unique identifier.
	/// This endpoint is accessible to users with the "Commuter" and "Driver" roles.
	/// </remarks>
	/// <param name="id">Unique identifier of the feedback.</param>
	/// <returns>A message indicating the result of the delete operation.</returns>
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