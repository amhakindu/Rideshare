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
			var mapperConfig = new MapperConfiguration(c =>
			{
				c.AddProfile<MappingProfile>();
			});
			_mapper = mapperConfig.CreateMapper();
			
			// _id = 1;

			_handler = new DeleteRateCommandHandler(_mockRepo.Object, _mapper);

		}


		[Fact]
		public async Task DeleteRate()
		{

			var Id = 1;

			var result = await _handler.Handle(new DeleteRateCommand() { RateId = Id }, CancellationToken.None);
			var rates = await _mockRepo.Object.RateRepository.GetAll();
			var exist = await _mockRepo.Object.RateRepository.Exists(Id);
			exist.ShouldBeFalse();
			
			// the count should be 1.
			rates.Count.ShouldBe(2);
		}

		
		[Fact]
		public async Task DeleteRate_Invalid_Id()
		{

			var Id = -1;
			try
			{
				var result = await _handler.Handle(new DeleteRateCommand() { RateId = Id }, CancellationToken.None);
			}
			catch (Exception ex) {
				var rates = await _mockRepo.Object.RateRepository.GetAll();
				rates.Count.ShouldBe(3);
			}
		}
	}
}


