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
              
        _mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        }).CreateMapper();

        _handler = new  GetRideRequestQueryHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task GetRideRequestValid()
    {
        var result = await _handler.Handle(new GetRideRequestQuery() { Id = 1, UserId = "sura123"}, CancellationToken.None);
        result.Value.Id.ShouldBe(1);
    }
       
    [Fact]
    public async Task GetRideRequestInvalid()
    {
         await Should.ThrowAsync<NotFoundException>(async () =>
    {
             await _handler.Handle(new GetRideRequestQuery() { Id = 3,UserId = "sura"}, CancellationToken.None);
    });
    }
}
