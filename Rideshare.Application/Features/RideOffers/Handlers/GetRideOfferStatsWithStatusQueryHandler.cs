using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetRideOfferStatsWithStatusQueryHandler: IRequestHandler<GetRideOfferStatsWithStatusQuery, BaseResponse<Dictionary<string, Dictionary<int, int>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRideOfferStatsWithStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<Dictionary<string, Dictionary<int, int>>>> Handle(GetRideOfferStatsWithStatusQuery command, CancellationToken cancellationToken)
        {
            var rideOfferStats = await _unitOfWork.RideOfferRepository.GetRideOfferStatisticsWithStatus(command.Year, command.Month);

            return new BaseResponse<Dictionary<string, Dictionary<int, int>>>{
                Success = true,
                Message = "RideOffers Statistics Fetching Successful",
                Value = rideOfferStats
            };
        }
    }
}