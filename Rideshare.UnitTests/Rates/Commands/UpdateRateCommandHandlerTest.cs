using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Features.Rates.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RateTest
{
	public class UpdateRateCommandHandlerTest
	{
		private IMapper _mapper { get; set; }
		private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
		private UpdateRateCommandHandler _handler { get; set; }

		public UpdateRateCommandHandlerTest()
		{
			_mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
			_mapper = new MapperConfiguration(c =>
			{
				c.AddProfile<MappingProfile>();
			}).CreateMapper();

			_handler = new UpdateRateCommandHandler(_mockUnitOfWork.Object, _mapper);
		}

		[Fact]
		public async Task UpdateRateValid()
		{

			var rateDto = new UpdateRateDto()
			{
				Id = 2,
				Rate = 3.7,
				UserId = "2",
				Description = "Description 2 Edited!"
			};
			await _handler.Handle(new UpdateRateCommand() { RateDto = rateDto }, CancellationToken.None);
			var rate = await _mockUnitOfWork.Object.RateRepository.Get(rateDto.Id);
			rate.Description.ShouldBe(rateDto.Description);
			rate.Rate.ShouldBe(rateDto.Rate);


		}

		[Fact]
		public async Task UpdateRate_Missing_Required_Fields()
		{
			// mising required values while updating.
			var rateDto = new UpdateRateDto()
			{
				Id = 10,
				Description = "description 10.",
			};

			ValidationException ex = await Should.ThrowAsync<ValidationException>(async () =>
			{
				var result = await _handler.Handle(new UpdateRateCommand() { RateDto = rateDto }, CancellationToken.None);
			});
		}
		
		[Fact]
		public async Task UpdateRate_InvalidDescriptionLength_ThrowsValidationException()
		{
			var rateDto = new UpdateRateDto()
			{
				Id = 1,
				Rate = 4.2,
				Description = new string('A', 401) // 401 characters is an invalid description length
			};

			ValidationException ex = await Should.ThrowAsync<ValidationException>(async () =>
			{
				await _handler.Handle(new UpdateRateCommand() { RateDto = rateDto }, CancellationToken.None);
			});

			// Assert the exception message or perform further assertions if needed
		}

		[Fact]
		public async Task UpdateRate_Negative_Rate_Value()
		{
			var rateDto = new UpdateRateDto()
			{
				Id = 1,
				Rate = -2, //=> -2 is an invalid rate value
				Description = "New description"
			};

			ValidationException ex = await Should.ThrowAsync<ValidationException>(async () =>
			{
				await _handler.Handle(new UpdateRateCommand() { RateDto = rateDto }, CancellationToken.None);
			});

		}

	}
}