using Moq;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using NetTopologySuite.Geometries;
using Rideshare.Application.Responses;
using Rideshare.Application.UnitTests.Mocks;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Domain.Models;

namespace Rideshare.UnitTests.Mocks;

public class MockRideRequestRepository
{
    public static Mock<IRideRequestRepository> GetRideRequestRepository()
    {
        var mockUserRepo = new MockUserRepository();
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
                UserId = "user1",
                User = new ApplicationUser(){
                PhoneNumber = "",
                FullName = ""
                }
            },
            new ()
            {
                Id=2,
                Origin = new GeographicalLocation(){Coordinate=new Point(38.7529, 9.0136){SRID=4326}},
                Destination =new GeographicalLocation(){Coordinate= new Point(38.7631, 9.0101){SRID=4326}},
                CurrentFare = 70,
                Status =  0,
                NumberOfSeats = 1,
                UserId = "user1",
                User = new ApplicationUser(){
                PhoneNumber = "",
                FullName = ""
                }
            },

            new ()
            {
                Id=3,
                Origin = new GeographicalLocation(){Coordinate= new Point(38.7631, 9.0101){SRID=4326}},
                Destination = new GeographicalLocation(){Coordinate= new Point(38.7529, 9.0136){SRID=4326}},
                CurrentFare = 70,
                Status =  0,
                NumberOfSeats = 1,
                UserId = "user1",
                User = new ApplicationUser(){
                PhoneNumber = "",
                FullName = ""
                }
                
            },
            new ()
            {
                Id=4,
                Origin = new GeographicalLocation(){Coordinate= new Point(-55, -54)},
                Destination = new GeographicalLocation(){Coordinate= new Point(54, 53)},
                CurrentFare = 70,
                Status =  0,
                NumberOfSeats = 1,
                UserId = "user1",
                 User = new ApplicationUser(){
                PhoneNumber = "",
                FullName = ""
                }
            },
        };
        var mockRepo = new Mock<IRideRequestRepository>();

        mockRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((int pageNumber, int pageSize) =>
        {
            var response = new PaginatedResponse<RideRequest>();
            var result = rideRequests.AsQueryable().Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            response.Count = rideRequests.Count();
            response.Value = result;
            return response;

        }
                        );
        mockRepo.Setup(r => r.Add(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>
        {
                rideRequest.Id = rideRequests.Count() + 1;
                rideRequests.Add(rideRequest);
                return 1;

        });

        mockRepo.Setup(r => r.Update(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>
        {
            rideRequests.Remove(rideRequests.Find(r => r.Id != rideRequest.Id)!);
            rideRequests.Add(rideRequest);
            return 1;
        });

        mockRepo.Setup(r => r.Delete(It.IsAny<RideRequest>())).ReturnsAsync((RideRequest rideRequest) =>

        {
            if (rideRequests.Exists(b => b.Id == rideRequest.Id && b.UserId == rideRequest.UserId))
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


        mockRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((int id) =>
        {
            return rideRequests.FirstOrDefault((r) => r.Id == id);
        });
        mockRepo.Setup(r => r.GetRideRequestWithDetail(It.IsAny<int>())).ReturnsAsync((int id) => {
            return rideRequests.FirstOrDefault((r) => r.Id == id);
        });
        


        mockRepo.Setup(r => r.SearchByGivenParameter(It.IsAny<int>(),It.IsAny<int>(),It.IsAny<Status>(),It.IsAny<double>(),It.IsAny<string>(),It.IsAny<string>())).ReturnsAsync((int PageNumber, int PageSize, Status? status, double? fare, string? name, string? phoneNumber) => {
            
            var response = new PaginatedResponse<RideRequest>();
            var result = rideRequests.AsQueryable().Skip((PageNumber - 1) * PageSize)
            .Where(item => (item.Status == (status ?? item.Status)))
            .Where(item => (item.User.PhoneNumber == (phoneNumber ?? item.User.PhoneNumber)))
            .Where(item => (item.CurrentFare <= (fare ?? item.CurrentFare)))
            .Where(item => (item.User.FullName == (name ?? item.User.FullName)))
            .Take(PageSize)
            .ToList();
            response.Count = rideRequests.Count();
            response.Value = result;
            return response;
        });

        return mockRepo;
    }
}
