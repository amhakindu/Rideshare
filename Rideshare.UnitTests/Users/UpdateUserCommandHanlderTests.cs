using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
using Xunit;

namespace Application.Security.Handlers.CommandHandlers.Tests
{
    public class UpdateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessfulResponseWithUpdatedUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var handler = new UpdateUserCommandHandler(userRepositoryMock.Object, mapperMock.Object);

            var userId = "user-id";
            var userUpdatingDto = new UserUpdatingDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                Age = 30
            };
            var updateUserCommand = new UpdateUserCommand
            {
                UserId = userId,
                User = userUpdatingDto
            };

            var existingUser = new ApplicationUser();
            var updatedUser = new ApplicationUser();

            userRepositoryMock.Setup(repo => repo.FindByIdAsync(userId)).ReturnsAsync(existingUser);
            userRepositoryMock.Setup(repo => repo.UpdateUserAsync(userId, It.IsAny<ApplicationUser>())).ReturnsAsync(updatedUser);

            // Act
            var result = await handler.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("User Created Successfully", result.Message);
            Assert.Equal(updatedUser, result.Value);
            userRepositoryMock.Verify(repo => repo.FindByIdAsync(userId), Times.Once);
            userRepositoryMock.Verify(repo => repo.UpdateUserAsync(userId, It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsException()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var handler = new UpdateUserCommandHandler(userRepositoryMock.Object, mapperMock.Object);

            var userId = "user-id";
            var userUpdatingDto = new UserUpdatingDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                Age = 30
            };
            var updateUserCommand = new UpdateUserCommand
            {
                UserId = userId,
                User = userUpdatingDto
            };

            userRepositoryMock.Setup(repo => repo.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(updateUserCommand, CancellationToken.None));
            userRepositoryMock.Verify(repo => repo.FindByIdAsync(userId), Times.Once);
            userRepositoryMock.Verify(repo => repo.UpdateUserAsync(userId, It.IsAny<ApplicationUser>()), Times.Never);
        }
    }
}
