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

namespace Rideshare.Application.UnitTest.RateTest.Command
{
	public class CreateRateCommandHandlerTest
	{

		private readonly IMapper _mapper;
		private readonly Mock<IUnitOfWork> _mockRepo;
		private readonly CreateRateDto _rateDto;
		private readonly CreateRateCommandHandler _handler;
		public CreateRateCommandHandlerTest()
		{
			_mockRepo = MockUnitOfWork.GetUnitOfWork();
			var mapperConfig = new MapperConfiguration(c =>
			{
				c.AddProfile<MappingProfile>();
			});
			_mapper = mapperConfig.CreateMapper();

			_rateDto = new CreateRateDto
			{
				Id = 1,
				Rate = 2.4,
				RaterId = 1,
				DriverId = 3,
				Description = "true",
			};

			_handler = new CreateRateCommandHandler( _mapper, _mockRepo.Object);

		}


		[Fact]
		public async Task CreateRate()
		{
			var result = await _handler.Handle(new CreateRateCommand() { RateDto = _rateDto }, CancellationToken.None);
			result.ShouldBeOfType<BaseResponse<int>>();
			result.Success.ShouldBeTrue();

			var rates = await _mockRepo.Object.RateRepository.GetAll();
			rates.Count.ShouldBe(1);

		}

		[Fact]
		public async Task InvalidRate_Added()
		{

			_rateDto.Id = -1;
			var result = await _handler.Handle(new CreateRateCommand() { RateDto = _rateDto }, CancellationToken.None);
			result.ShouldBeOfType<BaseResponse<int>>();
			result.Errors.ShouldBeNull();
			var rates = await _mockRepo.Object.RateRepository.GetAll();
			rates.Count.ShouldBe(1);

		}
	}
}



