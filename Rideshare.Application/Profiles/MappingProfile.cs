using AutoMapper;
using Rideshare.Domain.Models;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Common.Dtos.Common;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Infrastructure;

namespace Rideshare.Application.Profiles;

public class MappingProfile : Profile
{
    private GeocodingResolver _geocodingResolver;
    private FareResolver _fareResolver;
    private DurationResolver _durationResolver;
    private readonly VehicleResolver _vehicleResolver;
    private readonly DriverResolver _driverResolver;
    private readonly AvailableSeatResolver _availableSeatResolver;

    public MappingProfile(IMapboxService mapboxService, IUnitOfWork unitOfWork)
    {
        _geocodingResolver = new GeocodingResolver(mapboxService);
        _fareResolver = new FareResolver(mapboxService);
        _durationResolver = new DurationResolver(mapboxService);
        _vehicleResolver = new VehicleResolver(unitOfWork);
        _driverResolver = new DriverResolver(unitOfWork);
        _availableSeatResolver = new AvailableSeatResolver(unitOfWork);
        
        #region Vehicle Mappings

        CreateMap<Vehicle, VehicleDto>().ReverseMap();
        CreateMap<Vehicle, CreateVehicleDto>().ReverseMap();
        CreateMap<Vehicle, UpdateVehicleDto>().ReverseMap();

        #endregion Vehicle


        #region Driver Mappings
        CreateMap<Driver, DriverDetailDto>().ReverseMap();
        CreateMap<Driver, CreateDriverDto>().ReverseMap();
        CreateMap<Driver, UpdateDriverDto>().ReverseMap();
        CreateMap<Driver, ApproveDriverDto>().ReverseMap();
        #endregion Driver Mappings


        #region rideRequest Mappings

        CreateMap<RideRequest, RideRequestDto>()
            .ForMember(dto => dto.Status, opt => opt.MapFrom(riderequest => Enum.GetName(typeof(Status), riderequest.Status)))
            .ForMember(dto => dto.Origin, opt => opt.MapFrom(src => new LocationDto(){Longitude=src.Origin.Coordinate.X, Latitude=src.Origin.Coordinate.Y}))
            .ForMember(dto => dto.Destination, opt => opt.MapFrom(src => new LocationDto(){Longitude=src.Destination.Coordinate.X, Latitude=src.Destination.Coordinate.Y}));
        CreateMap<CreateRideRequestDto, RideRequest>();
        CreateMap<UpdateRideRequestDto, RideRequest>();

        #endregion rideRequest

        #region User Mappings
        CreateMap<ApplicationRole, RoleDto>()
                    .ReverseMap();
        CreateMap<ApplicationUser, UserCreationDto>()
       .ReverseMap();
        CreateMap<ApplicationUser, DriverCreatingDto>()
       .ReverseMap();
        CreateMap<ApplicationUser, UserCreationDto>()
       .ReverseMap();
        CreateMap<ApplicationUser, UserUpdatingDto>()
        .ReverseMap();
        CreateMap<ApplicationUser, UserDto>()
      
        .ReverseMap();
        CreateMap<ApplicationUser, UserDtoForAdmin>()
      
        .ReverseMap();
        CreateMap<ApplicationUser, AdminUserDto>()
       .ReverseMap();
         CreateMap<ApplicationUser, AdminCreationDto>()
       .ReverseMap();



        #endregion User
        #region Rate Mappings

        CreateMap<RateEntity, RateDto>().ReverseMap();
        CreateMap<RateEntity, CreateRateDto>().ReverseMap();
        CreateMap<RateEntity, UpdateRateDto>().ReverseMap();

        #endregion Rate

        #region
        CreateMap<Feedback, FeedbackDto>().ReverseMap();
        CreateMap<Feedback, CreateFeedbackDto>().ReverseMap();
        CreateMap<Feedback, UpdateFeedbackDto>().ReverseMap();
        #endregion

        #region _RideOffer Mappings

        CreateMap<CreateRideOfferDto, RideOffer>()
            .ForMember(dest => dest.EstimatedDuration, opt => opt.MapFrom(src => _durationResolver.Resolve(src, null, default, null)))
            .ForMember(dest => dest.EstimatedFare, opt => opt.MapFrom(src => _fareResolver.Resolve(src, null, default, null)))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => _vehicleResolver.Resolve(src, null, null, null)))
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => _driverResolver.Resolve(src, null, null, null)))
            .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => _availableSeatResolver.Resolve(src, null, default, null)));
            
        CreateMap<RideOffer, RideOfferDto>()
            .ForMember(dto => dto.OriginAddress, opt => opt.MapFrom(rideoffer => rideoffer.CurrentLocation.Address))
            .ForMember(dto => dto.DestinationAddress, opt => opt.MapFrom(rideoffer => rideoffer.Destination.Address))
            .ForMember(dto => dto.CurrentLocation, opt => opt.MapFrom(src => new LocationDto(){Longitude=src.CurrentLocation.Coordinate.X, Latitude=src.CurrentLocation.Coordinate.Y}))
            .ForMember(dto => dto.Destination, opt => opt.MapFrom(src => new LocationDto(){Longitude=src.Destination.Coordinate.X, Latitude=src.Destination.Coordinate.Y}))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(rideoffer => Enum.GetName(typeof(Status), rideoffer.Status)));

        CreateMap<RideOffer, CommuterViewOfRideOfferDto>()
            .ForMember(dto => dto.DriverName, opt => opt.MapFrom(rideoffer => rideoffer.Driver.User.FullName))
            .ForMember(dto => dto.DriverImageUrl, opt => opt.MapFrom(rideoffer => rideoffer.Driver.User.ProfilePicture))
            .ForMember(dto => dto.AverageRate, opt => opt.MapFrom(rideoffer => rideoffer.Driver.Rate.Last()))
            .ForMember(dto => dto.VehicleModel, opt => opt.MapFrom(rideoffer => rideoffer.Vehicle.Model))
            .ForMember(dto => dto.VehiclePlateNumber, opt => opt.MapFrom(rideoffer => rideoffer.Vehicle.PlateNumber))
            .ForMember(dto => dto.DriverPhoneNumber, opt => opt.MapFrom(rideoffer => rideoffer.Driver.User.PhoneNumber))
            .ForMember(dto => dto.CurrentLocation, opt => opt.MapFrom(src => new LocationDto(){Longitude=src.CurrentLocation.Coordinate.X, Latitude=src.CurrentLocation.Coordinate.Y}));

        CreateMap<RideOffer, RideOfferListDto>()
            .ForMember(dto => dto.OriginAddress, opt => opt.MapFrom(rideoffer => rideoffer.CurrentLocation.Address))
            .ForMember(dto => dto.DestinationAddress, opt => opt.MapFrom(rideoffer => rideoffer.Destination.Address))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(rideoffer => Enum.GetName(typeof(Status), rideoffer.Status)));

        
        CreateMap<LocationDto, GeographicalLocation>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => _geocodingResolver.Resolve(src, null, null, null)))
            .ForMember(geoLoc => geoLoc.Coordinate, opt => opt.MapFrom(src => new Point(src.Longitude, src.Latitude){SRID=4326}))
            .ReverseMap()
            .ConstructUsing(point => new LocationDto {Longitude=point.Coordinate.X, Latitude=point.Coordinate.Y});
            
        #endregion _RideOffer
    }
}

public class GeocodingResolver : IValueResolver<LocationDto, GeographicalLocation, string>
{
    private readonly IMapboxService _mapboxService;

    public GeocodingResolver(IMapboxService mapboxService)
    {
        _mapboxService = mapboxService;
    }

    public string Resolve(LocationDto source, GeographicalLocation destination, string destMember, ResolutionContext context)
    {
        Point loc = new Point(source.Longitude, source.Latitude);
        Point loc2 = new Point(source.Longitude, source.Latitude) { SRID = 4326 };
        Console.WriteLine("lattitude", source.Longitude.ToString(), "longtude", source.Latitude);
        Console.WriteLine("lattitude1", loc.X, "longtude1", loc.Y);
        Console.WriteLine("lattitude2", loc2.X, "longtude2", loc2.Y);
        var address = Task.Run(() => _mapboxService.GetAddressFromCoordinates(loc)).GetAwaiter().GetResult();
        Console.WriteLine("address", address);
        return address;
    }
}

public class DurationResolver : IValueResolver<CreateRideOfferDto, RideOffer, TimeSpan>
{
    private readonly IMapboxService _mapboxService;

    public DurationResolver(IMapboxService mapboxService)
    {
        _mapboxService = mapboxService;
    }

    public TimeSpan Resolve(CreateRideOfferDto source, RideOffer destination, TimeSpan destMember, ResolutionContext context)
    {
        Point origin = new Point(source.CurrentLocation.Longitude, source.CurrentLocation.Latitude);
        Point dest = new Point(source.Destination.Longitude, source.Destination.Latitude);

        var duration = Task.Run(() => _mapboxService.GetEstimatedDuration(origin, dest)).GetAwaiter().GetResult();
        return TimeSpan.FromSeconds(duration);
    }
}

public class FareResolver : IValueResolver<CreateRideOfferDto, RideOffer, double>
{
    private const double PRICE_PER_KM = 12;
    private const double PRICE_PER_MIN = 2;
    private readonly IMapboxService _mapboxService;

    public FareResolver(IMapboxService mapboxService)
    {
        _mapboxService = mapboxService;
    }

    public double Resolve(CreateRideOfferDto source, RideOffer destination, double destMember, ResolutionContext context)
    {
        Point origin = new Point(source.CurrentLocation.Longitude, source.CurrentLocation.Latitude);
        Point dest = new Point(source.Destination.Longitude, source.Destination.Latitude);

        var duration = TimeSpan.FromSeconds( Task.Run(() => _mapboxService.GetEstimatedDuration(origin, dest)).GetAwaiter().GetResult() );
        var distance = Task.Run(() => _mapboxService.GetDistance(origin, dest)).GetAwaiter().GetResult() / 1000.0;

        return (PRICE_PER_KM * distance + PRICE_PER_MIN * duration.Minutes) / 2.0;
    }
}

public class VehicleResolver : IValueResolver<CreateRideOfferDto, RideOffer, Vehicle>
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleResolver(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Vehicle Resolve(CreateRideOfferDto source, RideOffer destination, Vehicle destMember, ResolutionContext context)
    {
        return Task.Run(() => _unitOfWork.VehicleRepository.Get(source.VehicleID)).GetAwaiter().GetResult();
    }
}

public class DriverResolver : IValueResolver<CreateRideOfferDto, RideOffer, Driver>
{
    private readonly IUnitOfWork _unitOfWork;

    public DriverResolver(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Driver Resolve(CreateRideOfferDto source, RideOffer destination, Driver destMember, ResolutionContext context)
    {
        var vehicle =Task.Run(() => _unitOfWork.VehicleRepository.Get(source.VehicleID)).GetAwaiter().GetResult();
        return Task.Run(() => _unitOfWork.DriverRepository.Get(vehicle.DriverId)).GetAwaiter().GetResult();
    }
}

public class AvailableSeatResolver : IValueResolver<CreateRideOfferDto, RideOffer, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AvailableSeatResolver(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public int Resolve(CreateRideOfferDto source, RideOffer destination, int destMember, ResolutionContext context)
    {
        var vehicle = Task.Run(() => _unitOfWork.VehicleRepository.Get(source.VehicleID)).GetAwaiter().GetResult();
        return vehicle.NumberOfSeats;
    }
}