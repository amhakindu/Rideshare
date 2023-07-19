using Moq;
using Xunit;
using Shouldly;
using AutoMapper;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Profiles;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Common.Dtos.Common;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Commands;
using Rideshare.Application.Features.RideRequests.Handlers;

namespace Rideshare.UnitTests.RideRequests;

public class UpdateRideRequestCommandHandlerTests
{
        private IMapper _mapper { get; set; }
        
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
    private UpdateRideRequestCommandHandler _handler { get; set; }


    public UpdateRideRequestCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        var mapboxService = MockServices.GetMapboxService();
        _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
        .CreateMapper();

        _handler = new UpdateRideRequestCommandHandler(_mockUnitOfWork.Object, _mapper);
    }


    [Fact]
    public async Task UpdateRideRequestValid()
    {

        UpdateRideRequestDto rideRequestDto = new()
        {
            Id=1,
            Origin = new LocationDto(){
                   
                    Latitude = 24,
                    Longitude = 20
                },
                Destination = new LocationDto(){
                    Latitude = 10,
                    Longitude = 10
                },
            NumberOfSeats = 1,
            UserId = "user1"
            
                   };

        var result = await _handler.Handle(new UpdateRideRequestCommand() {RideRequestDto = rideRequestDto, UserId = rideRequestDto.UserId}, CancellationToken.None);
    

        (await _mockUnitOfWork.Object.RideRequestRepository.GetAll(1, 10)).Count.ShouldBe(4);
    }

      [Fact]
    public async Task UpdateRideRequestInValid()
    {

        UpdateRideRequestDto rideRequestDto = new()
        {
            Id=1,
            Origin = new LocationDto(){
                    Latitude = 20,
                    Longitude = 20
                },
                Destination = new LocationDto(){
                    Latitude = 20,
                    Longitude = 20
                },
            NumberOfSeats = 1,
            UserId = "user1"
            
                   };

              await Should.ThrowAsync<ValidationException>(async () =>
    {
           var result = await _handler.Handle(new UpdateRideRequestCommand() { RideRequestDto = rideRequestDto, UserId = rideRequestDto.UserId}, CancellationToken.None);
    });      
    }

    [Fact]
    public async Task UpdateRideRequestInValidTwo()
    {

        UpdateRideRequestDto rideRequestDto = new()
        {
            Id=1,
            Origin = new LocationDto(){
                   
                    Latitude = 24,
                    Longitude = 20
                },
                Destination = new LocationDto(){
                    Latitude = 10,
                    Longitude = 10
                },
            UserId = "user2"
            
                   };

              await Should.ThrowAsync<NotFoundException>(async () =>
    {
           var result = await _handler.Handle(new UpdateRideRequestCommand() { RideRequestDto = rideRequestDto, UserId = rideRequestDto.UserId}, CancellationToken.None);
    });  
    }
}
