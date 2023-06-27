using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideRequests.Commands;
using Rideshare.Application.Features.Tests.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.Infrastructure.Services;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideRequests;

public class CreateRideRequestCommandHandlerTests
{
    private readonly RideshareMatchingService _matchingService;

    private IMapper _mapper { get; set; }
       private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
       private CreateRideRequestCommandHandler _handler { get; set; }
       private Mock<IUserRepository> _mockUserRepository {get;set;}

       
       

       public CreateRideRequestCommandHandlerTests()
       {
              _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
              var mockMapboxService = MockServices.GetMapboxService();

              _matchingService = new RideshareMatchingService(
                     _mockUnitOfWork.Object,
                     mockMapboxService.Object,
                     500
              );
              
              var mapboxService = MockServices.GetMapboxService();

              _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
              .CreateMapper();

              _handler = new CreateRideRequestCommandHandler(_mockUnitOfWork.Object, _mapper, _matchingService);
       }
       
       
       [Fact]
       public async Task CreateRideRequestValid()
       {
              CreateRideRequestDto rideRequestDto = new()
              {
                  Origin = new LocationDto(){
                    Latitude = 8.9975,
                    Longitude = 38.7547
                },
                Destination = new LocationDto(){
                    Latitude = 9.0004,
                    Longitude = 38.7668
                },
                NumberOfSeats = 2,
                UserId = "user1"
              };
              int prevCount = (await _mockUnitOfWork.Object.RideRequestRepository.GetAll()).Count;
              var result = await _handler.Handle(new CreateRideRequestCommand() {  RideRequestDto = rideRequestDto }, CancellationToken.None);

              result.Value.ShouldNotBeNull();
              result.Value["MatchedRide"].ShouldNotBeNull();
              (await _mockUnitOfWork.Object.RideRequestRepository.GetAll()).Count.ShouldBe(prevCount+1);
       }
       
       [Fact]
       public async Task CreateRideRequestInvalid()
       {
       
              CreateRideRequestDto rideRequestDto = new()
              {
                Origin = new LocationDto(){
                    Latitude = 20,
                    Longitude = 20
                },
                Destination = new LocationDto(){
                    Latitude = 20,
                    Longitude = 20
                },
                NumberOfSeats = 1,
                UserId = "user2"

              };

             await Should.ThrowAsync<ValidationException>(async () =>
    {
           var result = await _handler.Handle(new CreateRideRequestCommand() { RideRequestDto = rideRequestDto }, CancellationToken.None);
    });   
       }


       
        
}