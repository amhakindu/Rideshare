using Moq;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.UnitTests.Mocks;

public class MockUnitOfWork
{
	public static Mock<IUnitOfWork> GetUnitOfWork()
	{
		var mockUow = new Mock<IUnitOfWork>();
		var mockTestEntityRepository = MockTestEntityRepository.GetTestEntityRepository();
		var mockRideRequestRepository = MockRideRequestRepository.GetRideRequestRepository();
		var mockDriverRepository = MockDriverRepository.GetDriverRepository();
		var mockRateRepository = MockRateRepository.GetRateRepository();
        var mockFeebackRepository = MockFeedbackRepository.GetFeedbackRepository();
        var mockVehicleRepository = MockVehicleRepository.GetVehicleRepository();

        mockUow.Setup(r => r.VehicleRepository).Returns(mockVehicleRepository.Object);
        mockUow.Setup(r => r.TestEntityRepository).Returns(mockTestEntityRepository.Object);
        mockUow.Setup(r => r.RideRequestRepository).Returns(mockRideRequestRepository.Object);
        mockUow.Setup(r => r.TestEntityRepository).Returns(mockTestEntityRepository.Object);
        mockUow.Setup(r => r.DriverRepository).Returns(mockDriverRepository.Object);
        mockUow.Setup(r => r.TestEntityRepository).Returns(mockTestEntityRepository.Object);
        mockUow.Setup(r => r.FeedbackRepository).Returns(mockFeebackRepository.Object);
		mockUow.Setup(r => r.RateRepository).Returns(mockRateRepository.Object);
        mockUow.Setup(r => r.Save()).ReturnsAsync(1);
        
        return mockUow;
    }
}
