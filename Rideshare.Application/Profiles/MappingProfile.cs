using AutoMapper;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
            #region TestEntity Mappings

            CreateMap<TestEntity, TestEntityDto>().ReverseMap();
            
            #endregion TestEntity

            #region rideRequest Mappings

            CreateMap<RideRequest, RideRequestDto>().ReverseMap();
            CreateMap<RideRequest, CreateRideRequestDto>().ReverseMap();
            CreateMap<RideRequest, UpdateRideRequestDto>().ReverseMap();
             
            
            #endregion rideRequest


    }
}
