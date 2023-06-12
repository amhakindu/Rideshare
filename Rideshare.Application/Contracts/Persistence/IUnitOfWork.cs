
namespace Rideshare.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    ITestEntityRepository TestEntityRepository { get; }
    IDriverRepository DriverRepository { get; }
    Task<int> Save(); 
}
