using Moq;
using NetTopologySuite.Geometries;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.UnitTests.Mocks;

public class MockRideRequestRepository
{
    

     public static Mock<IRideRequestRepository> GetRideRequestRepository()
    {
        var rideRequests = new List<RideRequest>
        {
            new ()
            {
                Id=1,
                Origin = new Point(15,18),
                Destination = new Point(15,20),
                CurrentFare = 60,
                Status =  0,

               
            },
            
            new ()
            {
                Id=2,
                Origin = new Point(25,18),
                Destination = new Point(75,20),
                CurrentFare = 70,
                Status =  0,
                
            }
        };
        
         var mockRepo = new Mock<IRideRequestRepository>();

        mockRepo.Setup(r => r.GetAll(1, 10)).ReturnsAsync(rideRequests);
        
        mockRepo.Setup(r => r.Add(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>
        {
            rideRequest.Id = rideRequests.Count() + 1;
            rideRequests.Add(rideRequest);
            return rideRequest.Id; 
        });

        mockRepo.Setup(r => r.Update(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>
        {
            var newRideRequests = rideRequests.Where((r) => r.Id != rideRequest.Id);
            rideRequests = newRideRequests.ToList();
            rideRequests.Add(rideRequest);
            return rideRequest.Id;
        });
        
        mockRepo.Setup(r => r.Delete(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>
        
        {
            if (rideRequests.Exists(b => b.Id == rideRequest.Id))
                rideRequests.Remove(rideRequests.Find(b => b.Id == rideRequest.Id)!);
                return rideRequest.Id;
        });

         mockRepo.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync((int id) =>
        {
            var rideRequest = rideRequests.FirstOrDefault((r) => r.Id == id);
            return rideRequest != null;
        });

        
        mockRepo.Setup(r => r.Get(It.IsAny<int>()))!.ReturnsAsync((int id) =>
        {
            return rideRequests.FirstOrDefault((r) => r.Id == id);
        });

        return mockRepo;
    }
}
