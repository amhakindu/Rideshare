using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
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
public class GetAllVehiclesQueryHandlerTests
{
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly GetAllVehiclesQueryHandler _handler;

    public GetAllVehiclesQueryHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork
            .GetUnitOfWork()
            .Object;
        var mapboxService = MockServices.GetMapboxService();

        _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork)); })
        .CreateMapper();

        _handler = new GetAllVehiclesQueryHandler(_mapper, _mockUnitOfWork);
    }

    [Fact]
    public async Task GetAllVehiclesValidTest()
    {
        var request = new GetAllVehiclesQuery();

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Value.ShouldNotBe(null);
        var expected = await _mockUnitOfWork.VehicleRepository.GetAll(1, 10);
        result.Value.Count.ShouldBe(expected.Count);
    }
}
