using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Queries;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetRideOfferStatsQueryHandler: IRequestHandler<GetRideOfferStatsQuery, BaseResponse<Dictionary<int, int>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRideOfferStatsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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