// using AutoMapper;
// using Moq;
// using Rideshare.Application.Common.Dtos.RideOffers;
// using Rideshare.Application.Contracts.Persistence;
// using Rideshare.Application.Exceptions;
// using Rideshare.Application.Features.RideOffers.Queries;
// using Rideshare.Application.Features.testEntitys.CQRS.Handlers;
// using Rideshare.Application.Profiles;
// using Rideshare.Application.Responses;
// using Rideshare.UnitTests.Mocks;
// using Shouldly;
// using Xunit;

// namespace Rideshare.UnitTests.RideOffers;

// public class GetRideOffersQueryHandlerTests
// {
//     private readonly IMapper _mapper;
//     private readonly Mock<IUnitOfWork> _mockUow;
//     private readonly GetRideOffersQueryHandler _handler;

//     public GetRideOffersQueryHandlerTests()
//     {
//         _mockUow = MockUnitOfWork.GetUnitOfWork();

//         var mapperConfig = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); });

//         _mapper = mapperConfig.CreateMapper();
//         _handler = new GetRideOffersQueryHandler(_mockUow.Object, _mapper);
//     }

//     [Fact]
//     public async Task ValidRideOffersFetchingTest(){
//         var command = new GetRideOffersQuery{DriverID="ASDF-1234-GHJK-5678"};

//         var response = await _handler.Handle(command, CancellationToken.None);

//         response.ShouldNotBeNull();
//         response.ShouldBeOfType<BaseResponse<IReadOnlyList<RideOfferListDto>>>();
//         response.Success.ShouldBeTrue();
//         response.Value.All(dto => dto.DriverID == "ASDF-1234-GHJK-5678");
//     }

//     [Fact]
//     public async Task InvalidRideOfferFetchingTest(){
//         var command = new GetRideOffersQuery{DriverID="345678"};
//         await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
//     }
// }
