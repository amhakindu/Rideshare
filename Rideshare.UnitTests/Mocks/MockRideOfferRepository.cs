using Moq;
using NetTopologySuite.Geometries;
using Rideshare.Application.Contracts.Persistence;
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
                Driver= new Driver(),
                Vehicle= new Vehicle(){Id=1},
                CurrentLocation = new GeographicalLocation(){Coordinate=new Point(38.7478, 8.9945){SRID=4326}},
                Destination = new GeographicalLocation(){Coordinate=new Point(38.7668, 9.0004){SRID=4326}},
                AvailableSeats = 2,
            },
            new RideOffer{
                Id = 2,
                Driver= new Driver(),
                Vehicle= new Vehicle(){Id=1},
                CurrentLocation = new GeographicalLocation(){Coordinate=new Point(38.7445, 9.0105){SRID=4326}},
                Destination = new GeographicalLocation(){Coordinate=new Point(38.7667, 9.0106){SRID=4326}},
                AvailableSeats = 2,
            }
        };
        Count = rideOffers.Count();
        var rideOfferRepo = new Mock<IRideOfferRepository>();
        
        rideOfferRepo.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((int id) => rideOffers.FirstOrDefault(o => o.Id == id));
        rideOfferRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(rideOffers);
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
                    rideOfferTemp.Vehicle = rideOffer.Vehicle;
                    rideOfferTemp.CurrentLocation = rideOffer.CurrentLocation;
                    rideOfferTemp.Destination = rideOffer.Destination;
                    rideOfferTemp.Status = rideOffer.Status;
                    return 1;
                }
            );
        return rideOfferRepo;
    }
}