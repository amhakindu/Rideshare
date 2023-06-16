using AutoMapper;
using MediatR;
using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Features.Auth.Handlers;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Rideshare.Application.Tests.Features.Auth.Handlers
{
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var smsSenderMock = new Mock<ISmsSender>();
            var mapperMock = new Mock<IMapper>();

            var command = new CreateUserCommand
            {
                UserCreationDto = new UserCreationDto
                {
                    Roles = new List<RoleDto>
                    {
                        new RoleDto { Id = "role1", Name = "Role 1" },
                        new RoleDto { Id = "role2", Name = "Role 2" }
                    },
                    UserName = "testuser",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "password123",
                    Email = "test@example.com",
                    PhoneNumber = "1234567890",
                    Age = 30
                }
            };

            var applicationUser = new ApplicationUser();
            var baseResponse = new BaseResponse<ApplicationUser>
            {
                Success = true,
                Message = "User Created Successfully",
                Value = applicationUser
            };

            userRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<List<ApplicationRole>>()))
                .ReturnsAsync(applicationUser);

            mapperMock.Setup(x => x.Map<ApplicationUser>(command.UserCreationDto)).Returns(applicationUser);

            smsSenderMock.Setup(x => x.SendSmsAsync(command.UserCreationDto.PhoneNumber, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var handler = new CreateUserCommandHandler(userRepositoryMock.Object, mapperMock.Object, smsSenderMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("User Created Successfully", response.Message);
            Assert.Equal(applicationUser, response.Value);

            userRepositoryMock.Verify(x => x.CreateUserAsync(applicationUser, command.UserCreationDto.Password, It.IsAny<List<ApplicationRole>>()), Times.Once);
            mapperMock.Verify(x => x.Map<ApplicationUser>(command.UserCreationDto), Times.Once);
            userRepositoryMock.Verify(x => x.UpdateUserAsync(applicationUser.Id, applicationUser), Times.Once);
        }
    }
}