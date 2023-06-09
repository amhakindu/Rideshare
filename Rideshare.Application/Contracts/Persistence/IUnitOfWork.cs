
namespace Rideshare.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    ITestEntityRepository TestEntityRepository { get; }
    Task<int> Save(); 
}
