using AutoMapper;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Profiles;

public class MappingProfile: Profile
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
    }
}
