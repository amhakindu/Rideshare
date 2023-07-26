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
			var validator = new GetCommutersCountStatisticsQueryValidator();

			// Act
			var validationResult = await validator.ValidateAsync(query);
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			validationResult.IsValid.Should().BeTrue();
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
			var validator = new GetCommutersCountStatisticsQueryValidator();

			// Act
			var validationResult = await validator.ValidateAsync(query);
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			validationResult.IsValid.Should().BeTrue();
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
			var validator = new GetCommutersCountStatisticsQueryValidator();

			// Act
			var validationResult = await validator.ValidateAsync(query);
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			validationResult.IsValid.Should().BeTrue();
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

			var query = new GetCommutersCountStatisticsQuery { Year = null, Month = 7 };
			var validator = new GetCommutersCountStatisticsQueryValidator();

			// Act
			var validationResult = await validator.ValidateAsync(query);
			ValidationException ex = await Should.ThrowAsync<ValidationException>(async () =>
			{
			var response = await handler.Handle(query, CancellationToken.None);
			
			// Assert
			validationResult.IsValid.Should().BeFalse();
			response.Should().NotBeNull();
			response.Success.Should().BeFalse();
			response.Message.Should().Be("If month is specified, year must also be specified.");
		
			});

		}

		[Fact]
		public async Task GetCommutersCountStatistics_Weekly_Invalid_InvalidMonth_ReturnsInvalidRequest()
		{
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			var query = new GetCommutersCountStatisticsQuery { Year = 2023, Month = 13 };
			var validator = new GetCommutersCountStatisticsQueryValidator();

			// Act
			var validationResult = await validator.ValidateAsync(query);
			
			ValidationException ex = await Should.ThrowAsync<ValidationException>(async () =>
			{
			var response = await handler.Handle(query, CancellationToken.None);
			
			// Assert
			validationResult.IsValid.Should().BeFalse();
			});
			
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
			var validator = new GetCommutersCountStatisticsQueryValidator();

			// Act
			var validationResult = await validator.ValidateAsync(query);
			var response = await handler.Handle(query, CancellationToken.None);

			// Assert
			validationResult.IsValid.Should().BeTrue();
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
			// Arrange
			var userRepositoryMock = new MockUserRepository();
			var mapperMock = new Mock<IMapper>();
			var handler = new GetCommutersCountStatisticsHandler(userRepositoryMock.Object, mapperMock.Object);

			// Provide an invalid year (year not in the range 2022 to current year)
			var query = new GetCommutersCountStatisticsQuery { Year = 2021, Month = 7 };
			var validator = new GetCommutersCountStatisticsQueryValidator();

			// Act
			var validationResult = await validator.ValidateAsync(query);
			
			ValidationException ex = await Should.ThrowAsync<ValidationException>(async () =>
			{
			// Assert
			var response = await handler.Handle(query, CancellationToken.None);
			validationResult.IsValid.Should().BeFalse();
			response.Should().NotBeNull();
			response.Success.Should().BeFalse();
			response.Message.Should().Be("One or more validation errors occurred.");
			});
		}
		
	}
}
