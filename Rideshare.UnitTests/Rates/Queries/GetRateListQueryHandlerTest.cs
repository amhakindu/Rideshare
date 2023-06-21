using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Handlers;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RateTest.Queries
{
	public class GetRateListQueryHandlerTest
	{


		private readonly IMapper _mapper;
		private readonly Mock<IUnitOfWork> _mockRepo;
		private readonly GetRateListQueryHandler _handler;
	   
		public GetRateListQueryHandlerTest()
		{
			_mockRepo = MockUnitOfWork.GetUnitOfWork();
			var mapboxService = MockServices.GetMapboxService();

			var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockRepo.Object)); });

			_mapper = mapperConfig.CreateMapper();

			_handler = new GetRateListQueryHandler (_mapper ,_mockRepo.Object);

		}


		 [Fact]
		public async Task GetRateListValid()
		{
			var result = await _handler.Handle(new GetRateListQuery() 
				{ 
					PageNumber = 1,  // Set the desired page number
					PageSize = 10    // Set the desired page size 
				}, CancellationToken.None);
				
			result.Success.ShouldBeTrue();
			result.Value?.Count.ShouldBe(3);
		}
	}
}

