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
    public ITestEntityRepository TestEntityRepository
    {
        get
        {
            if (_TestEntityRepository == null)
                _TestEntityRepository = new TestEntityRepository(_context);
            return _TestEntityRepository;
        }
    }

    private IVehicleRepository? _VehicleRepository;
    public IVehicleRepository VehicleRepository
    {
        get
        {
            if (_VehicleRepository == null)
                _VehicleRepository = new VehicleRepository(_context);
            return _VehicleRepository;
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
