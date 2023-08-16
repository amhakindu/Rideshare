using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Handlers;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Profiles;
using Rideshare.Domain.Common;
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
        var rideRequestFilterDto = new RideRequestsListFilterDto(){
            status = Status.WAITING,
            fare = 80,
            name = "",
            phoneNumber = ""
        };
        
        var result = await _handler.Handle(new GetRideRequestListQuery() { PageNumber = 1,PageSize = 10,RideRequestsListFilterDto = rideRequestFilterDto }, CancellationToken.None);
        
        result.Value?.Count.ShouldBe(4);
    }

     
}
