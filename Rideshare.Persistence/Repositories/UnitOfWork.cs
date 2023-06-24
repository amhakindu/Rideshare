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
	private IRateRepository? _RateRepository;
    private IFeedbackRepository? _FeedbackRepository;
    private IVehicleRepository? _VehicleRepository;
	private IConnectionRepository? _ConnectionRepository;
	
	
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

	 public IRateRepository RateRepository
	 {
		get
		{
			if (_RateRepository == null)
				_RateRepository = new RateRepository(_context);
			return _RateRepository;
		}
	}
    
    public IFeedbackRepository FeedbackRepository
    {
        get
        {
            if (_FeedbackRepository == null)
                _FeedbackRepository = new FeedbackRepository(_context);
            return _FeedbackRepository;
        }
    }


    public IVehicleRepository VehicleRepository
    {
        get
        {
            if (_VehicleRepository == null)
                _VehicleRepository = new VehicleRepository(_context);
            return _VehicleRepository;
        }
    }

    private IRideOfferRepository? _RideOfferRepository;
    public IRideOfferRepository RideOfferRepository
    {
        get
        {
            if (_RideOfferRepository == null)
                _RideOfferRepository = new RideOfferRepository(_context);
            return _RideOfferRepository;
        }
    }

	public IConnectionRepository ConnectionRepository
	{
		get
		{
			if (_ConnectionRepository == null)
				_ConnectionRepository = new ConnectionRepository(_context);
			return _ConnectionRepository;
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
