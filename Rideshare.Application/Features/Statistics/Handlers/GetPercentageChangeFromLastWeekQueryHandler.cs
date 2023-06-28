using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Domain.Common;
using Rideshare.Application.Common.Dtos.Statistics;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetPercentageChangeFromLastWeekQueryHandler: IRequestHandler<GetPercentageChangeFromLastWeekQuery, BaseResponse<IList<EntityCountChangeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPercentageChangeFromLastWeekQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IList<EntityCountChangeDto>>> Handle(GetPercentageChangeFromLastWeekQuery query, CancellationToken cancellationToken)
        {
            var rideofferChange = await _unitOfWork.RideOfferRepository.GetLastWeekPercentageChange();
            var riderequestChange = await _unitOfWork.RideRequestRepository.GetLastWeekPercentageChange();
            var driverChange = await _unitOfWork.DriverRepository.GetLastWeekPercentageChange();
            
            return new BaseResponse<IList<EntityCountChangeDto>>{
                Success = true,
                Message = "RideOffers Fetching Successful",
                Value = new List<EntityCountChangeDto>(){
                    new EntityCountChangeDto(){
                        Name="rideoffers",
                        CurrentCount=await _unitOfWork.RideOfferRepository.Count(),
                        PercentageChange=rideofferChange
                    },
                    new EntityCountChangeDto(){
                        Name="riderequests",
                        CurrentCount=await _unitOfWork.RideRequestRepository.Count(),
                        PercentageChange=riderequestChange
                    },
                    new EntityCountChangeDto(){
                        Name="rideoffers",
                        CurrentCount=await _unitOfWork.DriverRepository.Count(),
                        PercentageChange=driverChange
                    },
                }
            };
        }
    }
}