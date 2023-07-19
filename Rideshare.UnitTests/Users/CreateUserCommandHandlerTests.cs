using System.Threading;
using System.Threading.Tasks;
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
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userRepositoryMock = new MockUserRepository();
            var mapperMock = new Mock<IMapper>();
            var resourceManagerMock = new Mock<IResourceManager>();

            var handler = new CreateUserCommandHandler(userRepositoryMock.Object, mapperMock.Object, resourceManagerMock.Object);

            var command = new CreateUserCommand
            {
                UserCreationDto = new UserCreationDto
                {
                    
                    Roles = new RoleDto
                    { Id = "role1", Name = "Role 1" },
                    FullName = "testuser",
                    PhoneNumber = "+251123456789",
                    Age = 30
                }
            };


            var expectedUser = new ApplicationUser();
            var expectedUserDto = new UserDto();
            mapperMock.Setup(mapper => mapper.Map<ApplicationUser>(command.UserCreationDto)).Returns(expectedUser);
            mapperMock.Setup(mapper => mapper.Map<UserDto>(expectedUser)).Returns(expectedUserDto);


            var response = await handler.Handle(command, CancellationToken.None);

            Assert.True(response.Success);
            Assert.Equal(expectedUserDto, response.Value);
        }
    }
}
