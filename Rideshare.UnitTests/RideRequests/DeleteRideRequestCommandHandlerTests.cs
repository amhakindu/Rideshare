 using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideRequests.Handlers;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideRequests;

public class DeleteRideRequestCommandHandlerTests
{
     private IMapper _mapper { get; set; }
       private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
       private DeleteRideRequestCommandHandler _handler { get; set; }

       public  DeleteRideRequestCommandHandlerTests()
       {
              _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
              
              _mapper = new MapperConfiguration(c =>
              {
                     c.AddProfile<MappingProfile>();
              }).CreateMapper();

              _handler = new DeleteRideRequestCommandHandler(_mockUnitOfWork.Object, _mapper);
       }
       
       
       [Fact]
       public async Task DeleteRideRequestValid()
       {
       
              
               var result = await _handler.Handle(new DeleteRideRequestCommand() {  Id =  1,UserId = "sura123"}, CancellationToken.None);
               
              
              (await _mockUnitOfWork.Object.RideRequestRepository.GetAll(1, 10)).Count.ShouldBe(1);
       }
       
       [Fact]
       public async Task DeleteRideRequestInvalid()
       {
              
                 await Should.ThrowAsync<NotFoundException>(async () =>
    {
           var result = await _handler.Handle(new DeleteRideRequestCommand() { Id = 3 ,UserId = "sura123"}, CancellationToken.None);
    });    
       }
}
