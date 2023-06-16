using AutoMapper;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
            #region TestEntity Mappings

            CreateMap<TestEntity, TestEntityDto>().ReverseMap();

        #endregion TestEntity


        #region Driver Mappings
        CreateMap<Driver, DriverDetailDto>().ReverseMap();
        CreateMap<Driver, CreateDriverDto>().ReverseMap();  
        CreateMap<Driver, UpdateDriverDto>().ReverseMap();
        #endregion Driver Mappings
        

            #region rideRequest Mappings

            CreateMap<RideRequest, RideRequestDto>().ReverseMap();
            CreateMap<RideRequest, CreateRideRequestDto>().ReverseMap();
            CreateMap<RideRequest, UpdateRideRequestDto>().ReverseMap();
            CreateMap<Point , LocationDto>()
                .ConstructUsing(point => new LocationDto(){latitude=point.Y, longitude=point.X})
                .ReverseMap()
                .ConstructUsing(dto => new Point(dto.latitude, dto.longitude));

             
            
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

            #endregion User
    }
}
