using System;
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
            var expected = new List<int>{1, 1};
            var request = new GetCountByStatusRequest();

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Value.ShouldBeOfType<List<int>>();
            result.Value.Count.ShouldBe(2);
            result.Success.ShouldBe(true);
            result.Value.ShouldBe(expected);

        }
        
    }
}