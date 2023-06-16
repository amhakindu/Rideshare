using System.Runtime.InteropServices;
using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Movies.CQRS.Handlers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Profiles;
using Rideshare.Application.Responses;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideOffers;

public class GetRideOfferWithDetailsQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly GetRideOfferWithDetailsQueryHandler _handler;

    public GetRideOfferWithDetailsQueryHandlerTests()
    {
        _mockUow = MockUnitOfWork.GetUnitOfWork();

        var mapperConfig = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); });

        _mapper = mapperConfig.CreateMapper();
        _handler = new GetRideOfferWithDetailsQueryHandler(_mockUow.Object, _mapper);
    }

    [Fact]
    public async Task ValidRideOfferFetchingTest(){
        var command = new GetRideOfferWithDetailsQuery{RideOfferID=1};

        var response = await _handler.Handle(command, CancellationToken.None);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<BaseResponse<RideOfferDto>>();
        response.Success.ShouldBeTrue();

        response.Value.ShouldNotBeNull();
        response.Value.ShouldBeOfType<RideOfferDto>();
        response.Value.Id.ShouldBe(1);
    }
    
    [Fact]
    public async Task InvalidRideOfferFetchingTest(){
        var command = new GetRideOfferWithDetailsQuery{RideOfferID=1000000};
        await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
