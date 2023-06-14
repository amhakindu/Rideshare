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
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly GetAllVehiclesQueryHandler _handler;

    public GetAllVehiclesQueryHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();

        _handler = new GetAllVehiclesQueryHandler(_mapper, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAllVehiclesValidTest()
    {
        var request = new GetAllVehiclesQuery();

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Value.ShouldNotBe(null);
        result.Value.Count.ShouldBe(2);
    }
}
