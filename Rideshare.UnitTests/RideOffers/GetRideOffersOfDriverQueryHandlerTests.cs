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
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Features.testEntitys.CQRS.Handlers;

namespace Rideshare.UnitTests.RideOffers;

public class GetRideOffersQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly GetRideOffersOfDriverQueryHandler _handler;

    public GetRideOffersQueryHandlerTests()
    {
        _mockUow = MockUnitOfWork.GetUnitOfWork();
        var mapboxService = MockServices.GetMapboxService();

        var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUow.Object)); });

        _mapper = mapperConfig.CreateMapper();
        _handler = new GetRideOffersOfDriverQueryHandler(_mockUow.Object, _mapper);
    }

    [Fact]
    public async Task ValidFetchRideOffersByUserIdTest(){
        var command = new GetRideOffersOfDriverQuery{Id="user1"};

        var response = await _handler.Handle(command, CancellationToken.None);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<PaginatedResponse<RideOfferListDto>>();
        response.Success.ShouldBeTrue();
        response.Value.ShouldNotBeNull();
        response.Value.All(dto => dto.Driver.UserId == "user1");
    }

    [Fact]
    public async Task ValidFetchRideOffersByDriverIdTest(){
        var command = new GetRideOffersOfDriverQuery{Id=1};

        var response = await _handler.Handle(command, CancellationToken.None);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<PaginatedResponse<RideOfferListDto>>();
        response.Success.ShouldBeTrue();
        response.Value.ShouldNotBeNull();
        response.Value.All(dto => dto.Driver.Id == 1);
    }
}
