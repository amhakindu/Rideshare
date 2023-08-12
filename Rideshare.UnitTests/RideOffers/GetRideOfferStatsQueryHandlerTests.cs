using Moq;
using Xunit;
using Shouldly;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Features.testEntitys.CQRS.Handlers;

namespace Rideshare.UnitTests.RideOffers;

public class GetRideOfferStatsQueryHandlerTests
{
    public GetRideOfferStatsQueryHandlerTests()
    {
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnValidResponse()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var rideOfferRepositoryMock = new Mock<IRideOfferRepository>();

        var rideOfferStats = new Dictionary<int, int>
        {
            { 1, 10 },
            { 2, 20 },
        };

        rideOfferRepositoryMock.Setup(repo => repo.GetEntityStatistics(2023, 8))
                                .ReturnsAsync(rideOfferStats);

        unitOfWorkMock.Setup(uow => uow.RideOfferRepository).Returns(rideOfferRepositoryMock.Object);

        var handler = new GetRideOfferStatsQueryHandler(unitOfWorkMock.Object);

        var query = new GetRideOfferStatsQuery
        {
            Year = 2023,
            Month = 8
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.Value.ShouldBe(rideOfferStats);
    }
}
