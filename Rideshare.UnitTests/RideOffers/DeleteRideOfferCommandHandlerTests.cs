using AutoMapper;
using MediatR;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Features.testEntitys.CQRS.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.Application.Responses;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.RideOffers;

public class DeleteRideOfferCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly DeleteRideOfferCommandHandler _handler;

    public DeleteRideOfferCommandHandlerTests()
    {
        _mockUow = MockUnitOfWork.GetUnitOfWork();
        var mapboxService = MockServices.GetMapboxService();

        var mapperConfig = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUow.Object)); });

        _mapper = mapperConfig.CreateMapper();
        _handler = new DeleteRideOfferCommandHandler(_mockUow.Object, _mapper);
    }

    [Fact]
    public async Task ValidRideOfferDeletionTest(){
        var command = new DeleteRideOfferCommand{RideOfferID=1};

        int prevCount = MockRideOfferRepository.Count;
        var response = await _handler.Handle(command, CancellationToken.None);

        var deleted = !await _mockUow.Object.RideOfferRepository.Exists(1);

        response.ShouldNotBeNull();
        response.ShouldBeOfType<BaseResponse<Unit>>();
        response.Success.ShouldBeTrue();
        deleted.ShouldBeTrue();
    }

    [Fact]
    public async Task invalidRideOfferDeletion(){
        var command = new DeleteRideOfferCommand{RideOfferID=10};
        await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
