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

        _mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        }).CreateMapper();

        _handler = new GetRideRequestListQueryHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task GetRideRequestListValid()
    {
        var result = await _handler.Handle(new GetRideRequestListQuery() {UserId = "sura123"}, CancellationToken.None);
        result.Value?.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetRideRequestListInvalid()
    {
        var result = await _handler.Handle(new GetRideRequestListQuery() {UserId = "sura12"}, CancellationToken.None);
        result.Value?.Count.ShouldBe(0);
    }
}
