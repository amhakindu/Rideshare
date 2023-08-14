using AutoMapper;
using FluentAssertions;
using Moq;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Commuters.Handlers;
using Rideshare.Application.Features.Commuters.Queries;
using Rideshare.Application.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.Application.UnitTests.Commuters
{
	public class GetCommutersCountStatisticsHandlerTests
	{
		[Fact]
		public async Task GetCommutersCountStatistics_Yearly_Valid_ReturnsValidResponse()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			var query = new GetCommutersCountStatisticsQuery { Year = null, Month = null };

			// Act
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			response.Should().NotBeNull();
			response.Success.Should().BeTrue();
			response.Message.Should().Be("Fetched In Successfully");
			response.Value.Should().NotBeNull();
			response.Value.Should().BeEquivalentTo(new Dictionary<int, int> {
				{ 2022, 1 },
				{ 2023, 5 }
			});
		}

		[Fact]
		public async Task GetCommutersCountStatistics_Monthly_Valid_ReturnsValidResponse()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			var query = new GetCommutersCountStatisticsQuery { Year = 2023, Month = null };

			// Act
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			response.Should().NotBeNull();
			response.Success.Should().BeTrue();
			response.Message.Should().Be("Fetched In Successfully");
			response.Value.Should().NotBeNull();
			response.Value.Should().BeEquivalentTo(new Dictionary<int, int> {
				{ 1, 0 },
				{ 2, 0 },
				{ 3, 0 },
				{ 4, 0 },
				{ 5, 0 },
				{ 6, 0 },
				{ 7, 4 },
				{ 8, 1 },
				{ 9, 0 },
				{ 10, 0 },
				{ 11, 0 },
				{ 12, 0 },
			
			});
		}

		[Fact]
		public async Task GetCommutersCountStatistics_Weekly_Valid_ReturnsValidResponse()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			var query = new GetCommutersCountStatisticsQuery { Year = 2023, Month = 7 };

			// Act
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			response.Should().NotBeNull();
			response.Success.Should().BeTrue();
			response.Message.Should().Be("Fetched In Successfully");
			response.Value.Should().NotBeNull();
			response.Value.Should().BeEquivalentTo(new Dictionary<int, int> {
				{ 1, 2 },
				{ 2, 1 },
				{ 3, 1 },
				{ 4, 0 },
				{ 5, 0 }
			});
		}

		[Fact]
		public async Task GetCommutersCountStatistics_Weekly_Invalid_NoYear_ReturnsInvalidRequest()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			var query = new GetCommutersCountStatisticsQuery {Month = 7 };

			// Act

			var response = await handler.Handle(query, CancellationToken.None);
			
			var second = response;

		}

		[Fact]
		public async Task GetCommutersCountStatistics_Weekly_Invalid_InvalidMonth_ReturnsInvalidRequest()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			await Should.ThrowAsync<ValidationException> (async () => new GetCommutersCountStatisticsQuery { Year = 2023, Month = 13 });

			
			
		}
		
		[Fact]
		public async Task GetCommutersCountStatistics_Weekly_EmptyCounts_ReturnsValidResponse()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			// Provide a year and month combination where all weeks have zero counts
			var query = new GetCommutersCountStatisticsQuery { Year = 2023, Month = 5 };

			// Act
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			response.Should().NotBeNull();
			response.Success.Should().BeTrue();
			response.Message.Should().Be("Fetched In Successfully");
			response.Value.Should().NotBeNull();
			// Assert that all week counts are zero
			response.Value.Should().BeEquivalentTo(new Dictionary<int, int> {
				{ 1, 0 },
				{ 2, 0 },
				{ 3, 0 },
				{ 4, 0 },
				{ 5, 0 }
			});
		}
		
		[Fact]
		public async Task GetCommutersCountStatistics_Monthly_Invalid_InvalidYear_ReturnsInvalidRequest()
		{
			
			await Should.ThrowAsync<ValidationException> ( async  () => new GetCommutersCountStatisticsQuery { Year = 2011, Month = 7 });


			
			
		}
		
	}
}
