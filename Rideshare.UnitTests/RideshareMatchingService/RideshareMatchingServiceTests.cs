using System.Threading;
using Xunit;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Infrastructure.Services;
using Rideshare.UnitTests.Mocks;
using Moq;
using Shouldly;
using Rideshare.Domain.Entities;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.UnitTests.RideshareMatchingServices;


public class RideshareMatchingServiceTests
{
    private readonly IRideshareMatchingService _matchingService;
    private readonly Mock<MapboxService> _mapboxService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public RideshareMatchingServiceTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        var mockMapboxService = MockServices.GetMapboxService();

        _matchingService = new RideshareMatchingService(
            _mockUnitOfWork.Object,
            mockMapboxService.Object,
            500
        );
    }

    [Fact]
    public async Task OutsideRadiusShouldNotMatchRideOfferTest()
    {
        RideRequest testRequest = await _mockUnitOfWork.Object.RideRequestRepository.Get(4);
        await _matchingService.MatchWithRideoffer(testRequest);

        testRequest.MatchedRide.ShouldBeNull();
    }

    [Fact]
    public async Task ShouldMatchRideOfferTest()
    {
        RideRequest testRequest = await _mockUnitOfWork.Object.RideRequestRepository.Get(2);
        await _matchingService.MatchWithRideoffer(testRequest);

        testRequest.MatchedRide.ShouldNotBeNull();
        testRequest.MatchedRide.Id.ShouldBe(2);
    }

    [Fact]
    public async Task MinimumAvgDetourDistanceShouldMatchRideOfferVariantTest()
    {
        RideRequest testRequest = await _mockUnitOfWork.Object.RideRequestRepository.Get(1);
        await _matchingService.MatchWithRideoffer(testRequest);

        testRequest.MatchedRide.ShouldNotBeNull();
        testRequest.MatchedRide.Id.ShouldBe(1);
    }

    
    [Fact]
    public async Task OppositeDirectionShouldNotMatchRideOfferTest()
    {
        RideRequest testRequest = await _mockUnitOfWork.Object.RideRequestRepository.Get(3);
        await _matchingService.MatchWithRideoffer(testRequest);

        testRequest.MatchedRide.ShouldBeNull();
    }
    
    [Fact]
    public async Task NoAvailableSeatsShouldNotMatchRideOfferTest()
    {
        RideRequest testRequest = await _mockUnitOfWork.Object.RideRequestRepository.Get(2);
        RideOffer rideOffer = await _mockUnitOfWork.Object.RideOfferRepository.Get(2);
        rideOffer.AvailableSeats = 0;
        await _matchingService.MatchWithRideoffer(testRequest);

        testRequest.MatchedRide.ShouldBeNull();
    }
}
