
namespace Rideshare.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    ITestEntityRepository TestEntityRepository { get; }
    IVehicleRepository VehicleRepository { get; }
    Task<int> Save(); 
}
