using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetRideOfferStatsQueryHandler: IRequestHandler<GetRideOfferStatsQuery, BaseResponse<Dictionary<int, int>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public GetRideOfferStatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dictionary<int, int>>> Handle(GetRideOfferStatsQuery command, CancellationToken cancellationToken)
        {
            var rideOfferStats = await _unitOfWork.RideOfferRepository.GetEntityStatistics(command.Year, command.Month);

            return new BaseResponse<Dictionary<int, int>>{
                Success = true,
                Message = "RideOffers Statistics Fetching Successful",
                Value = rideOfferStats
            };
        }
    }
}