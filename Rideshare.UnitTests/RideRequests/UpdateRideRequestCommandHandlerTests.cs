using AutoMapper;
using Moq;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Handlers;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideRequests;

public class UpdateRideRequestCommandHandlerTests
{
        private IMapper _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
    private UpdateRideRequestCommandHandler _handler { get; set; }


    public UpdateRideRequestCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

        _mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        }).CreateMapper();

        _handler = new UpdateRideRequestCommandHandler(_mockUnitOfWork.Object, _mapper);
    }


    [Fact]
    public async Task UpdateCommentValid()
    {

        UpdateRideRequestDto rideRequestDto = new()
        {
            Id=1,
            Origin = new LocationDto(){
                    latitude = 20,
                    longitude = 20
                },
                Destination = new LocationDto(){
                    latitude = 10,
                    longitude = 10
                },
            Status =  0,
            CurrentFare = 100
            
                   };

        var result = await _handler.Handle(new UpdateRideRequestCommand() {RideRequestDto = rideRequestDto }, CancellationToken.None);

        var updatedRideRequest = await _mockUnitOfWork.Object.RideRequestRepository.Get(rideRequestDto.Id);

        updatedRideRequest.CurrentFare.ShouldBe(rideRequestDto.CurrentFare);
    

        (await _mockUnitOfWork.Object.RideRequestRepository.GetAll()).Count.ShouldBe(2);
    }
}
