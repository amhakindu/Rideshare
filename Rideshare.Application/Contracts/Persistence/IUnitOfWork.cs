
namespace Rideshare.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    ITestEntityRepository TestEntityRepository { get; }
    IRideRequestRepository RideRequestRepository { get;}
    Task<int> Save(); 
}
