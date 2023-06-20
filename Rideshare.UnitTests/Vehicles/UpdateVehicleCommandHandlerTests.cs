using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Features.Vehicles.Handlers;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rideshare.UnitTests.Vehicles;
public class UpdateVehicleCommandHandlerTests
{
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly UpdateVehicleCommandHandler _handler;

    public UpdateVehicleCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork
            .GetUnitOfWork()
            .Object;
        _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();

        _handler = new UpdateVehicleCommandHandler(_mapper, _mockUnitOfWork);
    }

    [Fact]
    public async Task UpdateVehicleValidTest()
    {
        var updateVehicle = new UpdateVehicleDto
        {
            Id = 1,
            PlateNumber = "vv401"
        };

        var command = new UpdateVehicleCommand { VehicleDto = updateVehicle };
        var result = await _handler.Handle(command, CancellationToken.None);
        var vehicle = await _mockUnitOfWork.VehicleRepository.Get(1);
        vehicle.PlateNumber.ShouldBe("vv401");
    }

    [Fact]
    public async Task UpdateVehicleInValidTest()
    {
        var updateVehicle = new UpdateVehicleDto
        {
            PlateNumber = "vv401"
        };

        var command = new UpdateVehicleCommand { VehicleDto = updateVehicle };

        await Should.ThrowAsync<ValidationException>(async () =>
        {
            var result = await _handler.Handle(command, CancellationToken.None);  
        });
    }
}
