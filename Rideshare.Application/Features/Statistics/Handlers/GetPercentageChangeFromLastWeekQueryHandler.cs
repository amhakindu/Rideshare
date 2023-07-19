using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Features.RideOffers.Queries;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetPercentageChangeFromLastWeekQueryHandler: IRequestHandler<GetPercentageChangeFromLastWeekQuery, BaseResponse<IList<EntityCountChangeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetPercentageChangeFromLastWeekQueryHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse<IList<EntityCountChangeDto>>> Handle(GetPercentageChangeFromLastWeekQuery query, CancellationToken cancellationToken)
        {
            var rideofferChange = await _unitOfWork.RideOfferRepository.GetLastWeekPercentageChange();
            var riderequestChange = await _unitOfWork.RideRequestRepository.GetLastWeekPercentageChange();
            var driverChange = await _unitOfWork.DriverRepository.GetLastWeekPercentageChange();
            var vehicleChange = await _unitOfWork.VehicleRepository.GetLastWeekPercentageChange();
            var commuterChange = await _userRepository.GetLastWeekPercentageChange();
            
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
                        Name="drivers",
                        CurrentCount=await _unitOfWork.DriverRepository.Count(),
                        PercentageChange=driverChange
                    },
                    new EntityCountChangeDto(){
                        Name="vehicles",
                        CurrentCount=await _unitOfWork.DriverRepository.Count(),
                        PercentageChange=vehicleChange
                    },
                    new EntityCountChangeDto(){
                        Name="commuters",
                        CurrentCount=await _userRepository.GetCommuterCount(),
                        PercentageChange=vehicleChange
                    },
                }
            };
        }
    }
}