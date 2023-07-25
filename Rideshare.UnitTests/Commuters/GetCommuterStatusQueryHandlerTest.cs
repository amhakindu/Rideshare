using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Commuters.Queries;
using Rideshare.Application.Responses;
using Rideshare.Application.UnitTests.Mocks;
using Rideshare.Domain.Models;
using Xunit;

namespace Rideshare.UnitTests.Commuters
{
	public class GetCommuterStatusQueryHandlerTests
	{
		[Fact]
		public async Task Handle_ValidRequest_ReturnsSuccessResponse()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();

			var mapperMock = new Mock<IMapper>();

			var handler = new GetCommuterStatusQueryHandler(userRepositoryMock.Object);

			var query = new GetCommuterStatusQuery();

			// Mock the user data
			var commuters = new List<ApplicationUser>
			{
				new ApplicationUser
				{
					Id = "user1",
					FullName = "John Doe",
					UserName = "johndoe",
					Email = "john@example.com",
					PhoneNumber = "1234567890",
					LastLogin = DateTime.Now.AddDays(-15)
				},
				new ApplicationUser
				{
					Id = "user2",
					FullName = "Jane Smith",
					UserName = "janesmith",
					Email = "jane@example.com",
					PhoneNumber = "9876543210",
					LastLogin = DateTime.Now.AddDays(-45)
				}
			};
			

		userRepositoryMock.Setup(repo => repo.GetUsersByRoleAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
			.ReturnsAsync((string role, int pageNumber, int pageSize) => 
			new PaginatedResponse<ApplicationUser>
			{
				Value = commuters,
				Count = commuters.Count
			});

			var expectedResponseDto = new CommuterStatusDto
			{
				ActiveCommuters = 1,
				IdleCommuters = 1
			};

			var expectedResponse = new BaseResponse<CommuterStatusDto>
			{
				Success = true,
				Message = "Commuters status count fetched Successfully!",
				Value = expectedResponseDto
			};

			// Act
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(response.Success);
			Assert.Equal("Commuters status count fetched Successfully!", response.Message);
			Assert.Equal(expectedResponseDto.ActiveCommuters, response.Value.ActiveCommuters);
			Assert.Equal(expectedResponseDto.IdleCommuters, response.Value.IdleCommuters);
		}
	}
}
