using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Feedbacks.Commands;
using Rideshare.Application.Features.Feedbacks.Handlers;
using Rideshare.Application.Features.Feedbacks.Queries;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rideshare.UnitTests.Feedbacks
{
	public class UpdateFeedbackCommandHandlerTest
	{
		private IMapper _mapper { get; set; }
		private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
		private UpdateFeedbackCommandHandler _handler { get; set; }

		public UpdateFeedbackCommandHandlerTest()
		{
			_mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
			var mapboxService = MockServices.GetMapboxService();
		   _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
			.CreateMapper();
			_handler = new UpdateFeedbackCommandHandler(_mockUnitOfWork.Object, _mapper);
		}

		[Fact]
		public async Task UpdateFeedbackValid()
		{

			var feedBackDto = new UpdateFeedbackDto()
			{
				Id = 1,
				Title = "Test Title updated",
				Content = "Test Content 3",
				Rating = 2
			};
			await _handler.Handle(new UpdateFeedbackCommand() { feedbackDto = feedBackDto }, CancellationToken.None);
			var feedback = await _mockUnitOfWork.Object.FeedbackRepository.Get(feedBackDto.Id);
			feedback.Title.ShouldBe(feedBackDto.Title);
			feedback.Content.ShouldBe(feedBackDto.Content);
			feedback.Rating.Equals(feedBackDto.Title);

		}

		[Fact]
		public async Task UpdateFeedbackInvalid()
		{
			// mising required values
			var feedbackDto = new UpdateFeedbackDto()
			{
				Id = 10,
				Content = "Test Content 5",
			};

			ValidationException ex = await Should.ThrowAsync<ValidationException>(async () =>
			{
				var result = await _handler.Handle(new UpdateFeedbackCommand() { feedbackDto = feedbackDto }, CancellationToken.None);
			});
		}
	}
}
