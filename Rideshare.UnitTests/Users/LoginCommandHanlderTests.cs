using System.Threading;
using System.Threading.Tasks;
using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Responses;
using Xunit;

namespace Rideshare.Application.Features.Auth.Handlers.Tests
{
    public class LoginCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessfulResponseWithLoginResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();

            var handler = new LoginCommandHandler(userRepositoryMock.Object);

            var userName = "john.doe";
            var password = "password";
            var loginRequest = new LoginRequest(userName, password);
            var loginCommand = new LoginCommand
            {
                LoginRequest = loginRequest
            };

            var loginResult = new LoginResponse("Logged In Successfully", "access_token", "refresh_token");

            userRepositoryMock.Setup(repo => repo.LoginAsync(userName, password)).ReturnsAsync(loginResult);

            // Act
            var result = await handler.Handle(loginCommand, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Logged In Successfully", result.Message);
            Assert.Equal(loginResult, result.Value);
            Assert.Equal("access_token", result.Value.AccessToken);
            Assert.Equal("refresh_token", result.Value.refreshToken);
            userRepositoryMock.Verify(repo => repo.LoginAsync(userName, password), Times.Once);
        }
    }
}
