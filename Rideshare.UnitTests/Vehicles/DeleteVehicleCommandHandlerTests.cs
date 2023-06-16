using AutoMapper;
using Moq;
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
public class DeleteVehicleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly DeleteVehicleCommandHandler _handler;

    public DeleteVehicleCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();

        _handler = new DeleteVehicleCommandHandler(_mapper, _mockUnitOfWork.Object);
    }
    [Fact]
    public async void DeleteVehicleValid()
    {

        var command = new DeleteVehicleCommand { VehicleId = 1 };

        var result = await _handler.Handle(command, CancellationToken.None);

        (await _mockUnitOfWork.Object.VehicleRepository.GetAll()).Count.ShouldBe(1);


    }

    [Fact]
    public async Task DeleteVehicleInValid()
    {
        var command = new DeleteVehicleCommand { VehicleId = 7 };

        await Should.ThrowAsync<NotFoundException>(async () =>
        {
            var result = await _handler.Handle(command, CancellationToken.None);
        });

        (await _mockUnitOfWork.Object.VehicleRepository.GetAll()).Count.ShouldBe(2);
    }
}
