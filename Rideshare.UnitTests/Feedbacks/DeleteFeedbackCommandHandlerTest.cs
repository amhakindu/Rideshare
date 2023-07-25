using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Commands;
using Rideshare.Application.Features.Feedbacks.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.Application.UnitTests.Mocks;
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
	public class DeleteFeedbackCommandHandlerTest
	{
		private IMapper _mapper { get; set; }
		private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
		private Mock<IUserRepository> _userRepositoryMock { get; set; }
		private DeleteFeedbackCommandHandler _handler { get; set; }


		public DeleteFeedbackCommandHandlerTest()
		{
			_mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

			var mapboxService = MockServices.GetMapboxService();
            var _userRepositoryMock = new MockUserRepository();

			_mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
			.CreateMapper();

			_handler = new DeleteFeedbackCommandHandler(_mockUnitOfWork.Object, _userRepositoryMock.Object, _mapper);
		}


		[Fact]
		public async Task DeleteFeedbackValid()
		{

			var Id = 1;

			var result = await _handler.Handle(new DeleteFeedbackCommand() { Id = Id }, CancellationToken.None);
			// the count should be 1
			var feeadbacks = await _mockUnitOfWork.Object.FeedbackRepository.GetAll(1, 10);
			var exist = await _mockUnitOfWork.Object.FeedbackRepository.Exists(1);
			exist.ShouldBeFalse();
			feeadbacks.Count.ShouldBe(1);
		}

		[Fact]
		public async Task DeleteFeedbackInvalid()
		{

			var Id = 10;
			try
			{
				var result = await _handler.Handle(new DeleteFeedbackCommand() { Id = Id }, CancellationToken.None);
			}
			catch (Exception ex) {
				var feedbacks = await _mockUnitOfWork.Object.FeedbackRepository.GetAll(1, 10);
				feedbacks.Count.ShouldBe(2);
			}
		}
	}
}
