using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Features.Vehicles.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rideshare.UnitTests.Vehicles;
public class CreateVehicleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly CreateVehicleCommandHandler _handler;

    public CreateVehicleCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();

        _handler = new CreateVehicleCommandHandler(_mapper, _mockUnitOfWork.Object);
    }

    [Fact]

    public async Task CreateVehicleValidTest()
    {

        var createVehicleDto = new CreateVehicleDto
        {
            PlateNumber = "AA507",
            NumberOfSeats = 4,
            Model = "FORD 230",
            Libre = "alibre-01",
            UserId = "03"
        };

        var command = new CreateVehicleCommand { VehicleDto = createVehicleDto };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Value.ShouldBeOfType<int>();
        (await _mockUnitOfWork.Object.VehicleRepository.GetAll()).Count.ShouldBe(3);
    }

    [Fact]
    public async Task CreateVehicleInvalidTest()
    {

        var createVehicleDto = new CreateVehicleDto
        {
            PlateNumber = "",
            NumberOfSeats = 4,
            Model = "FORD 230",
            Libre = "alibre-01",
            UserId = "03"
        };

        var command = new CreateVehicleCommand { VehicleDto = createVehicleDto };

        await Should.ThrowAsync<ValidationException>(async () =>
        {
            var result = await _handler.Handle(command, CancellationToken.None);
        });

        (await _mockUnitOfWork.Object.VehicleRepository.GetAll()).Count.ShouldBe(2);
    }
}
