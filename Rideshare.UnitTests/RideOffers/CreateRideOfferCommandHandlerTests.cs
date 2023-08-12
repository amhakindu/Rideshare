using Moq;
using Xunit;
using Shouldly;
using AutoMapper;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Profiles;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Common.Dtos.Common;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Features.testEntitys.CQRS.Handlers;

namespace Rideshare.UnitTests.RideOffers;

public class CreateRideOfferCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly CreateRideOfferCommandHandler _handler;

    public CreateRideOfferCommandHandlerTests()
    {
        _mockUow = MockUnitOfWork.GetUnitOfWork();
        var mapboxService = MockServices.GetMapboxService();

        var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUow.Object)); });

        _mapper = mapperConfig.CreateMapper();
        _handler = new CreateRideOfferCommandHandler(_mockUow.Object, _mapper);
    }

    [Fact]
    public async Task ValidRideOfferCreationTest()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=38.7445,
                    Latitude=9.0105
                },
                Destination = new LocationDto{
                    Longitude=38.7667,
                    Latitude=9.0106
                },
            },
            UserId="user3"
        };
        var prevCount = MockRideOfferRepository.Count;
        var response = await _handler.Handle(command, CancellationToken.None);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<BaseResponse<int>>();
        response.Success.ShouldBeTrue();
        response.Value.ShouldBe(prevCount+1);
    }
    
    [Fact]
    public async Task RideOfferCreationWithExistingNonCompletedRideOfferTest()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=38.7445,
                    Latitude=9.0105
                },
                Destination = new LocationDto{
                    Longitude=38.7667,
                    Latitude=9.0106
                },
            },
            UserId="user1"
        };
        var prevCount = MockRideOfferRepository.Count;
        var response = await _handler.Handle(command, CancellationToken.None);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<BaseResponse<int>>();
        response.Success.ShouldBeFalse();
        response.Value.ShouldBe(0);
    }
    
    [Fact]
    public async Task RideOfferCreationWithInvalidCurrentLocationLatitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=-123.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                },
            },
            UserId="user3"
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithInvalidCurrentLocationLongitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=195.0,
                    Latitude=-12.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                }
            },
            UserId="user3"
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithInvalidDestinationLatitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=-123.0
                }
            },
            UserId="user3"
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithInvalidDestinationLongitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=198.0,
                    Latitude=-12.0
                }
            },
            UserId="user3"
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithSameOriginAndDestination()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                }
            },
            UserId="user3"
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task RideOfferCreationWithNonExistingVehicle()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                VehicleID = 1000,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=-23.0
                }
            },
            UserId="user3"
        };
        await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}