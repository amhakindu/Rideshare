using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Vehicles.Handlers;
using Rideshare.Application.Features.Vehicles.Queries;
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
public class GetVehicleQueryHandlerTests
{
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly GetVehicleQueryHandler _handler;

    public GetVehicleQueryHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork
            .GetUnitOfWork()
            .Object;
        var mapboxService = MockServices.GetMapboxService();

        _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork)); })
        .CreateMapper();
        _handler = new GetVehicleQueryHandler(_mapper, _mockUnitOfWork);
    }

    [Fact]
    public async Task GetVehicleValidTest()
    {
        var request = new GetVehicleQuery { VehicleId = 1 };

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Value.ShouldNotBe(null);
        result.Value.ShouldBeOfType<VehicleDto>();
    }

    [Fact]
    public async Task GetVehicleInValidTest()
    {
        var request = new GetVehicleQuery { VehicleId = 10 };

        await Should.ThrowAsync<NotFoundException>(async () =>
        {
            var result = await _handler.Handle(request, CancellationToken.None);
        });
    }
}
