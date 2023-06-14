using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Rate.Handlers;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Profiles;
using Rideshare.Application.Responses;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

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
			_handler = new GetRateDetailQueryHandler( _mapper, _mockRepo.Object);

			Id = 1;


		}


		[Fact]
        public async Task GetRateDetailValid()
        {
            var result = await _handler.Handle(new GetRateDetailQuery() { RateId = 1 }, CancellationToken.None);
            result.Success.ShouldBeTrue();
        }

		[Fact]
        public async Task GetRateDetailInvalid()
        {
            NotFoundException ex = await Should.ThrowAsync<NotFoundException>(async () =>
            {
                var result = await _handler.Handle(new GetRateDetailQuery() { RateId = -1 }, CancellationToken.None);
            });
        }
    }
}