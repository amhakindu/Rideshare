using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Rate.Handlers;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.Rates.Queries
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
			var mapboxService = MockServices.GetMapboxService();

			var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockRepo.Object)); });

			_mapper = mapperConfig.CreateMapper();
			_handler = new GetRateDetailQueryHandler( _mapper, _mockRepo.Object);

			// Id = 2;


		}


		[Fact]
		public async Task GetRateDetailValid()
		{
			var result = await _handler.Handle(new GetRateDetailQuery() { RateId = 1 }, CancellationToken.None);
			result.Success.ShouldBeTrue();
			result.Value.ShouldBeOfType<RateDto>();
			
		}

		[Fact]
		public async Task GetRateDetail_IdNotExist()
		{
			Id = -1;
			NotFoundException ex = await Should.ThrowAsync<NotFoundException>(async () =>
			{
				var result = await _handler.Handle(new GetRateDetailQuery() { RateId = Id }, CancellationToken.None);
			});

			ex.Message.ShouldContain($"Rate with {Id} not found");
		}
	}
}