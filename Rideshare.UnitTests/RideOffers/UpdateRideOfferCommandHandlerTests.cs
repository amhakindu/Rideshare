using Moq;
using Xunit;
using MediatR;
using Shouldly;
using AutoMapper;
using Rideshare.UnitTests.Mocks;
using Rideshare.Domain.Entities;
using Rideshare.Application.Profiles;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Features.testEntitys.CQRS.Handlers;
using Rideshare.Application.Common.Dtos;

namespace Rideshare.UnitTests.RideOffers;

public class UpdateRideOfferCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly UpdateRideOfferCommandHandler _handler;

    public UpdateRideOfferCommandHandlerTests()
    {
        _mockUow = MockUnitOfWork.GetUnitOfWork();
        var mapboxService = MockServices.GetMapboxService();

        var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUow.Object)); });

        _mapper = mapperConfig.CreateMapper();
        _handler = new UpdateRideOfferCommandHandler(_mockUow.Object, _mapper);
    }

    [Fact]
    public async Task ValidRideOfferUpdateTest()
    {
        var newVehicleID = 2;
        var newOrigin = new LocationDto{
            Longitude=-56.0,
            Latitude=26.0
        };
        var newDestination = new LocationDto{
            Longitude=-108.0,
            Latitude=-82.0
        };
        var command = new UpdateRideOfferCommand
        {
            RideOfferDto = new UpdateRideOfferDto{
                Id=1,
                VehicleID = newVehicleID,
                CurrentLocation = newOrigin,
                Destination = newDestination
            }
        };

        var response = await _handler.Handle(command, CancellationToken.None);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<BaseResponse<Unit>>();
        response.Success.ShouldBeTrue();

        RideOffer temp = await _mockUow.Object.RideOfferRepository.Get(id: 1);
        temp.Vehicle.Id.ShouldBe(newVehicleID);
        temp.CurrentLocation.Coordinate.X.ShouldBe(newOrigin.Longitude);
        temp.CurrentLocation.Coordinate.Y.ShouldBe(newOrigin.Latitude);

        temp.Destination.Coordinate.X.ShouldBe(newDestination.Longitude);
        temp.Destination.Coordinate.Y.ShouldBe(newDestination.Latitude);
    }

    [Fact]
    public async Task RideOfferUpdateWithInvalidRideOfferID()
    {
        var command = new UpdateRideOfferCommand
        {
            RideOfferDto = new UpdateRideOfferDto{  Id=1234 }
        };
        await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task RideOfferUpdateWithInvalidCurrentLocationLatitude()
    {
        var command = new UpdateRideOfferCommand
        {
            RideOfferDto = new UpdateRideOfferDto{
                Id=2,
                CurrentLocation = new LocationDto{
                    Longitude=2.0,
                    Latitude=-123.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferUpdateWithInvalidCurrentLocationLongitude()
    {
        var command = new UpdateRideOfferCommand
        {
            RideOfferDto = new UpdateRideOfferDto{
                Id=2,
                CurrentLocation = new LocationDto{
                    Longitude=-234.0,
                    Latitude=13.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferUpdateWithInvalidDestinationLatitude()
    {
        var command = new UpdateRideOfferCommand
        {
            RideOfferDto = new UpdateRideOfferDto{
                Id=2,
                Destination = new LocationDto{
                    Longitude=1.0,
                    Latitude=-123.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task RideOfferUpdateWithInvalidDestinationLongitude()
    {
        var command = new UpdateRideOfferCommand
        {
            RideOfferDto = new UpdateRideOfferDto{
                Id=2,
                Destination = new LocationDto{
                    Longitude=234.0,
                    Latitude=-12.0
                }
            }
        };
        await Should.ThrowAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RideOfferUpdateWithNonExistingVehicle()
    {
        var command = new UpdateRideOfferCommand
        {
            RideOfferDto = new UpdateRideOfferDto{
                Id=1,
                VehicleID = 1000
            }
        };
        await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
