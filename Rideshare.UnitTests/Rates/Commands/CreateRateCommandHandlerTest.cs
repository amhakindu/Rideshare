using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Profiles;
using Moq;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Features.Rates.Handlers;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Features.Rates.Commands;
using Shouldly;
using Xunit;
using Rideshare.Application.Exceptions;

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
			var mapboxService = MockServices.GetMapboxService();

			var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockRepo.Object)); });

			_mapper = mapperConfig.CreateMapper();
			_handler = new CreateRateCommandHandler( _mapper, _mockRepo.Object);
			
			_rateDto = new CreateRateDto();

			_rateDto = new CreateRateDto
			{
				Id = 4,
				Rate = 2.4,
				UserId = "1",
				DriverId = 1,
				Description = "Description 1",
			};
			
		}

		[Fact]
		public async Task CreateRate()
		{
			
			await _handler.Handle(new CreateRateCommand() { RateDto = _rateDto }, CancellationToken.None);
			// The current rate.
			var rate = await _mockRepo.Object.RateRepository.Get(4);
			rate.ShouldNotBeNull();
			var rates = await _mockRepo.Object.RateRepository.GetAll(1, 10);
			rates.Count.ShouldBe(4);

		}

		 [Fact]
		public async Task CreateRate_InvalidRateValue()
		   {
			
			var rateDto = new CreateRateDto()
			{
				Id = 2,
				Rate = 12.4, //Rate must be between 1 and 10.
				UserId = "2",
				DriverId = 1,
				Description = "Description 1",

			};

			try
			{
				var result = await _handler.Handle(new CreateRateCommand() { RateDto = rateDto }, CancellationToken.None);
			}
			catch (Exception ex)
			{
				var rate = await _mockRepo.Object.RateRepository.Get(5);
				rate.ShouldBeNull();

				// count = 3
				var rates = await _mockRepo.Object.RateRepository.GetAll(1, 10);
				rates.Count.ShouldBe(3);
			}
		}
		
		[Fact]
		public async Task CreateRate_MissingRequiredFields()
			{
			var rateDto = new CreateRateDto
			{
				// Missing or null values for required properties
				Id = 3,
				UserId = "3",
				// DriverId is intentionally left as null
				Rate = 12.4, //Invalid Rate value (>10).
				Description = "Description 1"
			};

			try
			{
				await _handler.Handle(new CreateRateCommand { RateDto = rateDto }, CancellationToken.None);
			}
			catch (ValidationException ex)
			{
				// Assert that a validation exception is thrown
				ex.ShouldNotBeNull();

				// Verify that the rate is not added to the repository
				var rate = await _mockRepo.Object.RateRepository.Get(4);
				rate.ShouldBeNull();

				// Verify that the count of rates remains unchanged
				var rates = await _mockRepo.Object.RateRepository.GetAll(1, 10);
				rates.Count.ShouldBe(3);
				return;
			}

			// If no exception is thrown, the test should fail
			Assert.True(false, "ValidationException was expected but not thrown.");
		
		}

	}
}