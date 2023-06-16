using System.Threading;
using System.Threading.Tasks;
using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Features.Auth.Handlers;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
using Xunit;

namespace Rideshare.UnitTests.Application.Features.Auth.Handlers
{
    public class VerifyUserHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userId = "user1";
            var otpCode = "1234";
            var userRepositoryMock = new Mock<IUserRepository>();
            var handler = new VerifyUserHandler(userRepositoryMock.Object);
            var request = new VerifyUserCommand
            {
                UserId = userId,
                Code = new VerifyRequest(otpCode)
            };
            var user = new ApplicationUser
            {
                Id = userId,
                OtpCode = otpCode
            };
            userRepositoryMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("Verification Succeeded", response.Message);
            Assert.True(response.Value);
            userRepositoryMock.Verify(x => x.UpdateUserAsync(userId, user), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidRequest_ReturnsFailureResponse()
        {
            // Arrange
            var userId = "user1";
            var otpCode = "1234";
            var userRepositoryMock = new Mock<IUserRepository>();
            var handler = new VerifyUserHandler(userRepositoryMock.Object);
            var request = new VerifyUserCommand
            {
                UserId = userId,
                Code = new VerifyRequest(otpCode)
            };
            userRepositoryMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Verification Failed", response.Message);
            Assert.False(response.Value);
            userRepositoryMock.Verify(x => x.UpdateUserAsync(userId, It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_IncorrectOTPCode_ReturnsFailureResponse()
        {
            // Arrange
            var userId = "user1";
            var otpCode = "1234";
            var invalidOtpCode = "5678";
            var userRepositoryMock = new Mock<IUserRepository>();
            var handler = new VerifyUserHandler(userRepositoryMock.Object);
            var request = new VerifyUserCommand
            {
                UserId = userId,
                Code = new VerifyRequest(invalidOtpCode)
            };
            var user = new ApplicationUser
            {
                Id = userId,
                OtpCode = otpCode
            };
            userRepositoryMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Verification Failed", response.Message);
            Assert.False(response.Value);
            userRepositoryMock.Verify(x => x.UpdateUserAsync(userId, It.IsAny<ApplicationUser>()), Times.Never);
        }
    }
}
