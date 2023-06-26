using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Infrastructure;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetAllRideOffersQueryHandler: IRequestHandler<GetAllRideOffersQuery, BaseResponse<Dictionary<string, object>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public GetAllRideOffersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IMapboxService mapboxService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapboxService = mapboxService;
        }

        public async Task<BaseResponse<Dictionary<string, object>>> Handle(GetAllRideOffersQuery command, CancellationToken cancellationToken)
        {
            var driverRideOffers = await _unitOfWork.RideOfferRepository.GetAllPaginated(command.PageNumber, command.PageSize);
            return new BaseResponse<Dictionary<string, object>>{
                Success = true,
                Message = "RideOffers Fetching Successful",
                Value = new Dictionary<string, object>(){
                    {"count", driverRideOffers["count"]},
                    {"rideoffers", _mapper.Map<IReadOnlyList<RideOffer>, IReadOnlyList<RideOfferListDto>>((IReadOnlyList<RideOffer>)driverRideOffers["rideoffers"])}
                }
            };
        }
    }
}