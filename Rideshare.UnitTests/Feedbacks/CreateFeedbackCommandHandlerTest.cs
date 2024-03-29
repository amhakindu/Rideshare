﻿using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Commands;
using Rideshare.Application.Features.Feedbacks.Handlers;
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
	public class CreateFeedbackCommandHandlerTest
	{
		private IMapper _mapper { get; set; }
		private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
		private CreateFeedbackCommandHandler _handler { get; set; }

		public CreateFeedbackCommandHandlerTest()
		{
			_mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
			var mapboxService = MockServices.GetMapboxService();

			_mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
			.CreateMapper();


			_handler = new CreateFeedbackCommandHandler(_mockUnitOfWork.Object, _mapper);
		}

		[Fact]
		public async Task CreateFeedbackValid()
		{

			var feedBackDto = new CreateFeedbackDto()
			{
				Title = "Test Title 3",
				Content = "Test Content 3",
				UserId = "user2",
				Rating = 2
			};
			var res = await _handler.Handle(new CreateFeedBackCommand() { feedbackDto = feedBackDto }, CancellationToken.None);
			res.Value.ShouldBeOfType<int>();
			// should get the current review
			// var feedback = await _mockUnitOfWork.Object.FeedbackRepository.Get(2);
			// feedback.ShouldNotBeNull();
			// the count should be 3 because there are 2 that are already added
			var feedbacks = await _mockUnitOfWork.Object.FeedbackRepository.GetAll(1, 10);
			feedbacks.Count.ShouldBe(3);
		}

		[Fact]
		public async Task CreateFeedbackInvalid()
		{
			// mising required values
			var feedbackDto = new CreateFeedbackDto()
			{
				Content = "Test Content 5",
				UserId = "user1",

			};

			try
			{
				var result = await _handler.Handle(new CreateFeedBackCommand() { feedbackDto = feedbackDto }, CancellationToken.None);
			}
			catch (Exception ex)
			{
				var feedback = await _mockUnitOfWork.Object.FeedbackRepository.Get(5);
				feedback.ShouldBeNull();

				// the count should be 2
				var feedbacks = await _mockUnitOfWork.Object.FeedbackRepository.GetAll(1, 10);
				feedbacks.Count.ShouldBe(2);
			}
		}
	}
}

