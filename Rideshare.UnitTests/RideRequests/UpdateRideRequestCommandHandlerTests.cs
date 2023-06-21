using AutoMapper;
using Moq;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
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
                   
                    Latitude = 24,
                    Longitude = 20
                },
                Destination = new LocationDto(){
                    Latitude = 10,
                    Longitude = 10
                },
            Status =  0,
            CurrentFare = 100,
            NumberOfSeats = 1,
            UserId = "sura123"
            
                   };

        var result = await _handler.Handle(new UpdateRideRequestCommand() {RideRequestDto = rideRequestDto ,UserId = "sura123"}, CancellationToken.None);

        var updatedRideRequest = await _mockUnitOfWork.Object.RideRequestRepository.Get(rideRequestDto.Id);

        updatedRideRequest.CurrentFare.ShouldBe(rideRequestDto.CurrentFare);
    

        (await _mockUnitOfWork.Object.RideRequestRepository.GetAll(1, 10)).Count.ShouldBe(2);
    }

      [Fact]
    public async Task UpdateCommentInValid()
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
            Status =  0,
            CurrentFare = 100,
            NumberOfSeats = 1,
            UserId = "sula"
            
                   };

         await Should.ThrowAsync<ValidationException>(async () =>
    {
           var result = await _handler.Handle(new UpdateRideRequestCommand() { RideRequestDto = rideRequestDto,UserId = "sura123" }, CancellationToken.None);
    });    
        
    }
}
