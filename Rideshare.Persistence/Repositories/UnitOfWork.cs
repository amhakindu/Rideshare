using Microsoft.Extensions.Configuration;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RideshareDbContext _context;
    private readonly IConfiguration _configuration;

    

    public UnitOfWork(RideshareDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    private ITestEntityRepository? _TestEntityRepository;
    private IRideRequestRepository? _RideRequestRepository;

    private IDriverRepository _DriverRepository;
    public ITestEntityRepository TestEntityRepository
    {
        get
        {
            if (_TestEntityRepository == null)
                _TestEntityRepository = new TestEntityRepository(_context);
            return _TestEntityRepository;
        }
    }
    public IDriverRepository DriverRepository
    {
        get
        {
            if (_DriverRepository == null)
                _DriverRepository = new DriverRepository(_context);
            return _DriverRepository;
        }
    }

    public IRideRequestRepository RideRequestRepository  
      {
        get
        {
            if (_RideRequestRepository == null)
                _RideRequestRepository = new RideRequestRepository(_context);
            return _RideRequestRepository;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<int> Save()
    {
        return await _context.SaveChangesAsync();
    }
}
