using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetCountForEachStatusQueryHandler: IRequestHandler<GetCountForEachStatusQuery, BaseResponse<Dictionary<string, int>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCountForEachStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<Dictionary<string, int>>> Handle(GetCountForEachStatusQuery query, CancellationToken cancellationToken)
        {
            Dictionary<string, int> statusCount = await _unitOfWork.RideOfferRepository.GetRideOfferCountForEachStatus();
            foreach (var name in Enum.GetNames(typeof(Status)))
            {
                if(!statusCount.ContainsKey(name))
                    statusCount[name] = 0;
            }
            return new BaseResponse<Dictionary<string, int>>{
                Success = true,
                Message = "RideOffers Fetching Successful",
                Value = statusCount
            };
        }
    }
}