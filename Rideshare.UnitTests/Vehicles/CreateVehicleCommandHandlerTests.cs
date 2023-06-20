using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
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
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;
    private readonly IFormFile _mockPDF;
    private readonly CreateVehicleCommandHandler _handler;

    public CreateVehicleCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork
            .GetUnitOfWork()
            .Object;
        _resourceManager = MockResourceManager
            .GetResourceManager()
            .Object;
        _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();
        _mockPDF = MockPDF.GetMockPDF();
        _handler = new CreateVehicleCommandHandler(_mapper, _mockUnitOfWork, _resourceManager);
    }

    [Fact]
    public async Task CreateVehicleValidTest()
    {

        var createVehicleDto = new CreateVehicleDto
        {
            PlateNumber = "AA507",
            NumberOfSeats = 4,
            Model = "FORD 230",
            Libre = _mockPDF,
            DriverId = 2
        };

        var command = new CreateVehicleCommand { VehicleDto = createVehicleDto };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Value.ShouldBeOfType<int>();
        var vehicle = await _mockUnitOfWork.VehicleRepository.Get((int) result.Value);
        vehicle.ShouldNotBeNull();
        vehicle.Libre.ShouldBe($"http://cloudinary.com/{_mockPDF.FileName}");
    }

    [Fact]
    public async Task CreateVehicleInvalidTest()
    {

        var createVehicleDto = new CreateVehicleDto
        {
            PlateNumber = "",
            NumberOfSeats = 4,
            Model = "FORD 230",
            Libre = _mockPDF,
            DriverId = 2
        };

        var command = new CreateVehicleCommand { VehicleDto = createVehicleDto };

        await Should.ThrowAsync<ValidationException>(async () =>
        {
            var result = await _handler.Handle(command, CancellationToken.None);
        });

        (await _mockUnitOfWork.VehicleRepository.GetAll(1, 10)).Count.ShouldBe(2);
    }
}
