using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;

namespace Rideshare.Application.Features.RideOffers.Handlers
{
    public class GetNoTopModelRideOfferQueryHandler : IRequestHandler<GetNoTopModelRideOfferQuery, BaseResponse<IReadOnlyList<ModelAndCountDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNoTopModelRideOfferQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<IReadOnlyList<ModelAndCountDto>>> Handle(GetNoTopModelRideOfferQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IReadOnlyList<ModelAndCountDto>>();
            var result = await _unitOfWork.RideOfferRepository.NoTopModelOffers();
            response.Value = result;
            response.Message = "Number of rideoffers for top 10 models are succesfully fetched";
            return response;
        }
    }
}
