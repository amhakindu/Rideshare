using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rate.Handlers;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Profiles;
using Rideshare.Application.Responses;
using Rideshare.UnitTests.Mocks;
using Shouldly;

namespace Rideshare.UnitTests.RateTest.Queries
{
    public class GetRateDetailQueryHandlerTest
	{


		private readonly IMapper _mapper;
		private readonly Mock<IUnitOfWork> _mockRepo;
		private int Id;
		private readonly GetRateDetailQueryHandler _handler;
		public GetRateDetailQueryHandlerTest()
		{
			_mockRepo = MockUnitOfWork.GetUnitOfWork();
			var mapperConfig = new MapperConfiguration(c =>
			{
				c.AddProfile<MappingProfile>();
			});
			_mapper = mapperConfig.CreateMapper();

			Id = 1;

			_handler = new GetRateDetailQueryHandler( _mapper, _mockRepo.Object);

		}


		[Fact]
		public async Task GetRateDetail()
		{
			var result = await _handler.Handle(new GetRateDetailQuery() { RateId = Id }, CancellationToken.None);
			result.ShouldBeOfType<BaseResponse<RateDto>>();
			result.Success.ShouldBeTrue();
			result.Value.ShouldBeOfType<RateDto>();
		}

		[Fact]
		public async Task GetNonExistingRate()
		{

			Id = 0;
			var result = await _handler.Handle(new GetRateDetailQuery() { RateId = Id }, CancellationToken.None);
			result.ShouldBeOfType<BaseResponse<RateDto>>();
			result.Success.ShouldBeTrue();
			result.Value.ShouldBe(null);
		}
	}
}
