
using Rideshare.Application.Contracts.Identity;

namespace Rideshare.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IRideRequestRepository RideRequestRepository { get;}
	IDriverRepository DriverRepository { get; }
	IRateRepository RateRepository { get; }
    IFeedbackRepository FeedbackRepository { get; }
    IVehicleRepository VehicleRepository { get; }
    IRideOfferRepository RideOfferRepository { get; }
    IConnectionRepository ConnectionRepository { get; }
    Task<int> Save(); 
}
