using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
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
       public async Task DeleteCommentValid()
       {
       
              
               var result = await _handler.Handle(new DeleteRideRequestCommand() {  Id =  1}, CancellationToken.None);
               
              
              (await _mockUnitOfWork.Object.RideRequestRepository.GetAll()).Count.ShouldBe(1);
       }
       
       [Fact]
       public async Task DeleteCommentInvalid()
       {
              
              var result = await _handler.Handle(new DeleteRideRequestCommand() { Id =  2}, CancellationToken.None);
              
              (await _mockUnitOfWork.Object.RideRequestRepository.GetAll()).Count.ShouldBe(1);
       }
}
