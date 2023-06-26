using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Handlers;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideRequests;

public class GetRideRequestListQueryHandlerTests 
{
    private IMapper _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
    private GetRideRequestListQueryHandler _handler { get; set; }

    public GetRideRequestListQueryHandlerTests ()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

        var mapboxService = MockServices.GetMapboxService();

        _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
        .CreateMapper();

        _handler = new GetRideRequestListQueryHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task GetRideRequestListValid()
    {
        var result = await _handler.Handle(new GetRideRequestListQuery() {UserId = "user1"}, CancellationToken.None);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(4);
    }

    [Fact]
    public async Task GetRideRequestListInvalid()
    {
        var result = await _handler.Handle(new GetRideRequestListQuery() {UserId = "user12333"}, CancellationToken.None);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(0);
    }
}
