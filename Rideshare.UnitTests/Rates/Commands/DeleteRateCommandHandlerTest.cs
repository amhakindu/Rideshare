using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Profiles;
using Moq;
using Shouldly;
using Rideshare.Application.Features.Rates.Handlers;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Features.Rates.Commands;
using Xunit;

namespace Rideshare.UnitTests.RateTest.Commands
{
	public class DeleteRateCommandHandlerTest
	{

		private readonly IMapper _mapper;
		private readonly Mock<IUnitOfWork> _mockRepo;
		// private int _id { get; set; }
		private readonly DeleteRateCommandHandler _handler;
		private readonly CreateRateDto _rateDto; 
		public DeleteRateCommandHandlerTest()
		{
			_mockRepo = MockUnitOfWork.GetUnitOfWork();
			var mapboxService = MockServices.GetMapboxService();

			var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockRepo.Object)); });

			_mapper = mapperConfig.CreateMapper();
			

			_handler = new DeleteRateCommandHandler(_mockRepo.Object, _mapper);

		}


		[Fact]
		public async Task DeleteRate()
		{

			var id = 1;
			var userId = "1";

			var result = await _handler.Handle(new DeleteRateCommand() { Id = id, UserId = userId }, CancellationToken.None);
			var rates = await _mockRepo.Object.RateRepository.GetAll(1, 10);
			var exist = await _mockRepo.Object.RateRepository.Exists(id);
			exist.ShouldBeFalse();
			
			// the count should be 1.
			rates.Count.ShouldBe(2);
		}
		
		[Fact]
		public async Task DeleteRate_UnAuthorized()
		{
			var id = 1;
			var userId = "2"; //Un-authorized user to update this resource.

			UnauthorizedAccessException ex = await Should.ThrowAsync<UnauthorizedAccessException>(async () =>
			{
				await _handler.Handle(new DeleteRateCommand() { Id = id, UserId = userId }, CancellationToken.None);
			});
			
		}
		
		

		
		[Fact]
		public async Task DeleteRate_Invalid_Id()
		{

			var id = -1;
			var userId = "1";
			try
			{
				var result = await _handler.Handle(new DeleteRateCommand() { Id = id, UserId = userId }, CancellationToken.None);
			}
			catch (Exception ex) {
				var rates = await _mockRepo.Object.RateRepository.GetAll(1, 10);
				rates.Count.ShouldBe(3);
			}
		}
	}
}


