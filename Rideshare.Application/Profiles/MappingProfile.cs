using AutoMapper;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Common.Dtos.Feedbacks;
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
        

            #region rideRequest Mappings

            CreateMap<RideRequest, RideRequestDto>().ReverseMap();
            CreateMap<RideRequest, CreateRideRequestDto>().ReverseMap();
            CreateMap<RideRequest, UpdateRideRequestDto>().ReverseMap();
            CreateMap<Point , LocationDto>()
                .ConstructUsing(point => new LocationDto(){latitude=point.Y, longitude=point.X})
                .ReverseMap()
                .ConstructUsing(dto => new Point(dto.latitude, dto.longitude));

            #endregion rideRequest

			CreateMap<RateEntity, RateDto>().ReverseMap();
			CreateMap<RateEntity, CreateRateDto>().ReverseMap();
			CreateMap<RateEntity, UpdateRateDto>().ReverseMap();
			
			

        #region
        CreateMap<Feedback, FeedbackDto>().ReverseMap();
        CreateMap<Feedback, CreateFeedbackDto>().ReverseMap();
        CreateMap<Feedback, UpdateFeedbackDto>().ReverseMap();
        #endregion
    }
}
