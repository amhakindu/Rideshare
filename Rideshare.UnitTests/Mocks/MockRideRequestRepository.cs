using Moq;
using NetTopologySuite.Geometries;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
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
                Origin = new GeographicalLocation(){Coordinate=new Point(38.7547, 8.9975){SRID=4326}},
                Destination = new GeographicalLocation(){Coordinate=new Point(38.7668, 9.0004){SRID=4326}},
                CurrentFare = 60,
                Status =  0,
                NumberOfSeats = 2,
                UserId = "user1"
            },
            new ()
            {
                Id=2,
                Origin = new GeographicalLocation(){Coordinate=new Point(38.7529, 9.0136){SRID=4326}},
                Destination =new GeographicalLocation(){Coordinate= new Point(38.7631, 9.0101){SRID=4326}},
                CurrentFare = 70,
                Status =  0,
                NumberOfSeats = 1,
                UserId = "user1"
            },
            
            new ()
            {
                Id=3,
                Origin = new GeographicalLocation(){Coordinate= new Point(38.7631, 9.0101){SRID=4326}},
                Destination = new GeographicalLocation(){Coordinate= new Point(38.7529, 9.0136){SRID=4326}},
                CurrentFare = 70,
                Status =  0,
                NumberOfSeats = 1,
                UserId = "user1"
            },
            new ()
            {
                Id=4,
                Origin = new GeographicalLocation(){Coordinate= new Point(-55, -54)},
                Destination = new GeographicalLocation(){Coordinate= new Point(54, 53)},
                CurrentFare = 70,
                Status =  0,
                NumberOfSeats = 1,
                UserId = "user1"
            },
            // new ()
            // {
            //     Id=1,
            //     Origin = new Point(38.7547, 8.9975){SRID=4326},
            //     Destination = new Point(38.7668, 9.0004){SRID=4326},
            // },
            // new ()
            // {
            //     Id=2,
            //     Origin = new Point(38.7529, 9.0136){SRID=4326},
            //     Destination = new Point(38.7631, 9.0101){SRID=4326},
            // },
        };
        var mockRepo = new Mock<IRideRequestRepository>();

        mockRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(rideRequests);
        
        mockRepo.Setup(r => r.Add(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>
        {
            rideRequest.Id = rideRequests.Count() + 1;
            rideRequests.Add(rideRequest);
            return 1;
        });

        mockRepo.Setup(r => r.Update(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>
        {
            var newRideRequests = rideRequests.Where((r) => r.Id != rideRequest.Id);
            if (rideRequests.Count() == newRideRequests.Count())
            {
                return 0;
            }
            rideRequests = newRideRequests.ToList();
            rideRequests.Add(rideRequest);
            return 1;
        });

        mockRepo.Setup(r => r.Delete(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>

        {
            if (rideRequests.Exists(b => b.Id == rideRequest.Id))
            {
                rideRequests.Remove(rideRequests.Find(b => b.Id == rideRequest.Id)!);
                return 1;
            }
            return 0;
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
