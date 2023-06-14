using AutoMapper;
using Rideshare.Application.Common.Dtos.Drivers;
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


        #region Driver Mappings
        CreateMap<Driver, DriverDetailDto>().ReverseMap();
        CreateMap<Driver, CreateDriverDto>().ReverseMap();  
        CreateMap<Driver, UpdateDriverDto>().ReverseMap();
        #endregion Driver Mappings
    }
}
