using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideRequests.Handlers;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideRequests;

public class GetRideRequestQueryHandlerTests
{
    private IMapper _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork{ get; set; }  
    private GetRideRequestQueryHandler _handler { get; set; }

    public GetRideRequestQueryHandlerTests()
    { 
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
              
        var mapboxService = MockServices.GetMapboxService();

        _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
        .CreateMapper();

        _handler = new  GetRideRequestQueryHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task GetRideRequestValid()
    {
        var result = await _handler.Handle(new GetRideRequestQuery() { Id = 1, UserId = "user1"}, CancellationToken.None);
        result.Value.Id.ShouldBe(1);
    }
       
    [Fact]
    public async Task GetRideRequestInvalidOne()
    {
         await Should.ThrowAsync<NotFoundException>(async () =>
    {
             await _handler.Handle(new GetRideRequestQuery() { Id = 30,UserId = "user1"}, CancellationToken.None);
    });
    }

    [Fact]
      public async Task GetRideRequestInvalidTwo()
    {
         await Should.ThrowAsync<NotFoundException>(async () =>
    {
             await _handler.Handle(new GetRideRequestQuery() { Id = 30,UserId = "user1"}, CancellationToken.None);
    });
    }
}
