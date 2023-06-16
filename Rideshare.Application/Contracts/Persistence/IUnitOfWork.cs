
namespace Rideshare.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
	ITestEntityRepository TestEntityRepository { get; }
	IRideRequestRepository RideRequestRepository { get;}
	IDriverRepository DriverRepository { get; }
	IRateRepository RateRepository { get; }

	Task<int> Save(); 
}
