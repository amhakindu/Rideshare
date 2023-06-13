using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Features.Tests.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideRequests;

public class CreateRideRequestCommandHandlerTests
{
    
       private IMapper _mapper { get; set; }
       private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
       private CreateRideRequestCommandHandler _handler { get; set; }

       
       

       public CreateRideRequestCommandHandlerTests()
       {
              _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
              
              _mapper = new MapperConfiguration(c =>
              {
                     c.AddProfile<MappingProfile>();
              }).CreateMapper();

              _handler = new CreateRideRequestCommandHandler(_mockUnitOfWork.Object, _mapper);
       }
       
       
       [Fact]
       public async Task CreateRideRequestValid()
       {
       
              CreateRideRequestDto rideRequestDto = new()
              {
                  Origin = new LocationDto(){
                    x = 20,
                    y = 20
                },
                Destination = new LocationDto(){
                    x = 20,
                    y = 20
                },
                Status =  0,
                CurrentFare = 65,
              };
              
              var result = await _handler.Handle(new CreateRideRequestCommand() {  RideRequestDto = rideRequestDto }, CancellationToken.None);
              
              result.Value.ShouldBeEquivalentTo(3);

              
              (await _mockUnitOfWork.Object.RideRequestRepository.GetAll()).Count.ShouldBe(3);
       }
       
       [Fact]
       public async Task CreateRideRequestInvalid()
       {
       
              CreateRideRequestDto rideRequestDto = new()
              {
                Origin = new LocationDto(){
                    x = 20,
                    y = 20
                },
                Destination = new LocationDto(){
                    x = 20,
                    y = 20
                },
                Status =  0,
                CurrentFare = 65,

              };
              
              var result = await _handler.Handle(new CreateRideRequestCommand() { RideRequestDto = rideRequestDto }, CancellationToken.None);
              
              result.Value.ShouldBe(3);
       }
}
