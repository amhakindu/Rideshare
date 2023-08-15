using System;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Handlers;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.Drivers
{
    public class GetCountByStatusRequestHandlerTests
    {

        private readonly IUnitOfWork _mockUnitOfWork;
        private readonly GetCountByStatusRequestHandler _handler;



        public GetCountByStatusRequestHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork().Object;
            _handler = new GetCountByStatusRequestHandler(_mockUnitOfWork);
        }


        [Fact]
        public async void GetCountByStatusRequestValid()
        {
            var expected = new StatusDto{Active = 1, Idle = 2};
            var request = new GetCountByStatusRequest();

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Value.ShouldBeOfType<StatusDto>();
            result.Success.ShouldBe(true);
            result.Value.Active.ShouldBe(expected.Active);
            result.Value.Idle.ShouldBe(expected.Idle);

        }
        
    }
}