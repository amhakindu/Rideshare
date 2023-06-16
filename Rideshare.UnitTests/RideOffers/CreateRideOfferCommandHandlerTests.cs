using Moq;
using Xunit;
using Shouldly;
using AutoMapper;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Profiles;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
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

        var mapperConfig = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); });

        _mapper = mapperConfig.CreateMapper();
        _handler = new CreateRideOfferCommandHandler(_mockUow.Object, _mapper);
    }

    [Fact]
    public async Task ValidRideOfferCreationTest()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "ASDF-1234-GHJK-5678",
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=10.0,
                    Latitude=2.0
                },
            }
        };
        var prevCount = MockRideOfferRepository.Count;
        var response = await _handler.Handle(command, CancellationToken.None);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<BaseResponse<int>>();
        response.Success.ShouldBeTrue();
        response.Value.ShouldBe(prevCount+1);
    }
    
    [Fact]
    public async Task RideOfferCreationWithInvalidCurrentLocationLatitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "ASDF-1234-GHJK-5678",
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=-123.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                },
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithInvalidCurrentLocationLongitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "ASDF-1234-GHJK-5678",
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=195.0,
                    Latitude=-12.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithInvalidDestinationLatitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "ASDF-1234-GHJK-5678",
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=-123.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithInvalidDestinationLongitude()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "ASDF-1234-GHJK-5678",
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=198.0,
                    Latitude=-12.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferCreationWithSameOriginAndDestination()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "ASDF-1234-GHJK-5678",
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=2.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task RideOfferCreationWithNonExistingDriver()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "45678",
                VehicleID = 1,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=-23.0
                }
            }
        };
        await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task RideOfferCreationWithNonExistingVehicle()
    {
        var command = new CreateRideOfferCommand
        {
            RideOfferDto = new CreateRideOfferDto{
                DriverID = "ASDF-1234-GHJK-5678",
                VehicleID = 1000,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=2.0
                },
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=-23.0
                }
            }
        };
        await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}