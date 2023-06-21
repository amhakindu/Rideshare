using System.Threading;
using System.Threading.Tasks;
using Application.Security.Handlers.CommandHandlers;
using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Features.Auth.Handlers;
using Rideshare.Application.Responses;
using Rideshare.Application.UnitTests.Mocks;
using Rideshare.Domain.Models;
using Xunit;

namespace Rideshare.UnitTests.Users
{
    public class UpdateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
           
            var userRepositoryMock = new MockUserRepository();
            var mapperMock = new Mock<IMapper>();
            var resourceManagerMock = new Mock<IResourceManager>();

            var command = new UpdateUserCommand
            {
                UserId = "user_id",
                User = new UserUpdatingDto
                {
                    FullName = "John Doe",
                    Age = 30
                }
            };

            var updatedUser = new ApplicationUser
            {
                Id = "user_id",
                FullName = "John Doe",
                Age = 30
            };

            userRepositoryMock.Setup(u => u.UpdateUserAsync(command.UserId, It.IsAny<ApplicationUser>())).ReturnsAsync(updatedUser);
            mapperMock.Setup(m => m.Map<UserDto>(updatedUser)).Returns(new UserDto { FullName = "John Doe", Age = 30 });
            var handler = new UpdateUserCommandHandler(userRepositoryMock.Object, mapperMock.Object, resourceManagerMock.Object);

            var response = await handler.Handle(command, CancellationToken.None);

        
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("User Updated Successfully", response.Message);
            Assert.NotNull(response.Value);
            Assert.Equal("John Doe", response.Value.FullName);
            Assert.Equal(30, response.Value.Age);
        }

       
    }
}
