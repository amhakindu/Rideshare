using Moq;
using NetTopologySuite.Geometries;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.UnitTests.Mocks;

public class MockRideOfferRepository
{
    public static int Count{ get; set;} = 0;
    public static Mock<IRideOfferRepository> GetRideOfferRepository(){
        var rideOffers = new List<RideOffer>{
            new RideOffer{
                Id = 1,
                DriverID="QWER-1234-TYUI-5678",
                VehicleID=1,
                CurrentLocation = new Point(10.0, 30.0),
                Destination = new Point(10.0, -37.0),
                Status = Status.WAITING,
                AvailableSeats = 2,
                EstimatedFare = 203.00,
                EstimatedDuration = TimeSpan.FromHours(2.0)
            },
            new RideOffer{
                Id = 2,
                DriverID="ASDF-1234-GHJK-5678",
                VehicleID=1,
                CurrentLocation = new Point(10.0, 30.0),
                Destination = new Point(10.0, -37.0),
                Status = Status.ONROUTE,
                AvailableSeats = 2,
                EstimatedFare = 203.00,
                EstimatedDuration = TimeSpan.FromHours(2.0)
            },
            new RideOffer{
                Id = 3,
                DriverID="ZXCV-1234-BNMM-5678",
                VehicleID=1,
                CurrentLocation = new Point(10.0, 30.0),
                Destination = new Point(10.0, -37.0),
                Status = Status.COMPLETED,
                AvailableSeats = 2,
                EstimatedFare = 203.00,
                EstimatedDuration = TimeSpan.FromHours(2.0)
            },
        };
        Count = rideOffers.Count();
        var rideOfferRepo = new Mock<IRideOfferRepository>();
        
        rideOfferRepo.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((int id) => rideOffers.FirstOrDefault(o => o.Id == id));
        rideOfferRepo.Setup(repo => repo.GetAll()).ReturnsAsync(()=>rideOffers);
        rideOfferRepo.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync((int id) => rideOffers.Exists(o => o.Id == id));

        rideOfferRepo.Setup(repo => repo.Add(It.IsAny<RideOffer>())).ReturnsAsync((RideOffer rideOffer)=>{
            MockRideOfferRepository.Count += 1;
            rideOffer.Id = MockRideOfferRepository.Count;
            rideOffers.Add(rideOffer);
            return 1;
        });

        rideOfferRepo.Setup(repo => repo.Delete(It.IsAny<RideOffer>()))
            .ReturnsAsync(
                (RideOffer rideOffer) => {
                    int cnt = rideOffers.RemoveAll(b => b.Id == rideOffer.Id);
                    Count -= cnt;
                    return cnt;
                }
            );

        rideOfferRepo.Setup(repo => repo.Update(It.IsAny<RideOffer>()))
            .ReturnsAsync(
                (RideOffer rideOffer) => {
                    RideOffer? rideOfferTemp = rideOffers.FirstOrDefault(b => b.Id == rideOffer.Id);
                    if(rideOfferTemp == null)
                        return 0;
                    rideOfferTemp.VehicleID = rideOffer.VehicleID;
                    rideOfferTemp.CurrentLocation = rideOffer.CurrentLocation;
                    rideOfferTemp.Destination = rideOffer.Destination;
                    rideOfferTemp.Status = rideOffer.Status;
                    return 1;
                }
            );
        return rideOfferRepo;
    }
}