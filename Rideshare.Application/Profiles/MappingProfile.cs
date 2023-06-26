using AutoMapper;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;
using Rideshare.Application.Common.Dtos;

namespace Rideshare.Application.Profiles;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        #region TestEntity Mappings

        CreateMap<TestEntity, TestEntityDto>().ReverseMap();

        #endregion TestEntity

        #region Vehicle Mappings

        CreateMap<Vehicle, VehicleDto>().ReverseMap();
        CreateMap<Vehicle, CreateVehicleDto>().ReverseMap();
        CreateMap<Vehicle, UpdateVehicleDto>().ReverseMap();

        #endregion Vehicle


        #region Driver Mappings
        CreateMap<Driver, DriverDetailDto>().ReverseMap();
        CreateMap<Driver, CreateDriverDto>().ReverseMap();
        CreateMap<Driver, UpdateDriverDto>().ReverseMap();
        #endregion Driver Mappings


        #region rideRequest Mappings

        CreateMap<RideRequest, RideRequestDto>().ReverseMap();
        CreateMap<RideRequest, CreateRideRequestDto>().ReverseMap();
        CreateMap<RideRequest, UpdateRideRequestDto>().ReverseMap();
        CreateMap<Point, LocationDto>()
            .ConstructUsing(point => new LocationDto() { Latitude = point.Y, Longitude = point.X })
            .ReverseMap()
            .ConstructUsing(dto => new Point(dto.Latitude, dto.Longitude));

        #endregion rideRequest

        #region User Mappings
        CreateMap<ApplicationRole, RoleDto>()
                    .ReverseMap();
        CreateMap<ApplicationUser, UserCreationDto>()
       .ReverseMap();
        CreateMap<ApplicationUser, UserCreationDto>()
       .ReverseMap();
        CreateMap<ApplicationUser, UserUpdatingDto>()
        .ReverseMap();
        CreateMap<ApplicationUser, UserDto>()
      
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

        CreateMap<RideOffer, RideOfferDto>().ReverseMap();
        CreateMap<RideOffer, RideOfferListDto>().ReverseMap();
        CreateMap<RideOffer, CreateRideOfferDto>().ReverseMap();
        CreateMap<Point, LocationDto>()
            .ConstructUsing(point => new LocationDto { Longitude = point.X, Latitude = point.Y })
            .ReverseMap()
            .ConstructUsing(dto => new Point(dto.Longitude, dto.Latitude));

        #endregion _RideOffer
    }
}
