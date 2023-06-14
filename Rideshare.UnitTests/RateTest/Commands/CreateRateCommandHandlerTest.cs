using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Profiles;
using Moq;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Features.Rates.Handlers;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Rates.Commands;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RateTest.Commands
{
	public class CreateRateCommandHandlerTest
	{

		private readonly IMapper _mapper;
		private readonly Mock<IUnitOfWork> _mockRepo;
		private CreateRateDto _rateDto;
		private readonly CreateRateCommandHandler _handler;
		public CreateRateCommandHandlerTest()
		{
			_mockRepo = MockUnitOfWork.GetUnitOfWork();
			var mapperConfig = new MapperConfiguration(c =>
			{
				c.AddProfile<MappingProfile>();
			});
			_mapper = mapperConfig.CreateMapper();
			_handler = new CreateRateCommandHandler( _mapper, _mockRepo.Object);
			
			_rateDto = new CreateRateDto();

		}

		[Fact]
		public async Task CreateRate()
		{
			_rateDto = new CreateRateDto
			{
				Id = 4,
				Rate = 2.4,
				RaterId = 1,
				DriverId = 3,
				Description = "Description 1",
			};
			
			
			await _handler.Handle(new CreateRateCommand() { RateDto = _rateDto }, CancellationToken.None);
			// The current rate.
			var rate = await _mockRepo.Object.RateRepository.Get(4);
			rate.ShouldNotBeNull();
			var rates = await _mockRepo.Object.RateRepository.GetAll();
			rates.Count.ShouldBe(4);

		}

		// [Fact]
		// public async Task InvalidRate_Added()
		// {
		// 	_rateDto.Rate = 11.0; // Set an invalid rate value
		// 	var result = await _handler.Handle(new CreateRateCommand() { RateDto = _rateDto }, CancellationToken.None);
		// 	result.ShouldBeOfType<BaseResponse<int>>();
		// 	result.Errors.ShouldNotBeNull();
		// 	var rates = await _mockRepo.Object.RateRepository.GetAll();
		// 	rates.Count.ShouldBe(2);
		// }

	}
}



