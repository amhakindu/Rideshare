using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestByTimeQueryHandler : IRequestHandler<GetRideRequestByTimeQuery,BaseResponse<Dictionary<int, int>>>
{

    IUnitOfWork _unitOfWork;
    IMapper _mapper;

    public GetRideRequestByTimeQueryHandler(IUnitOfWork  unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BaseResponse<Dictionary<int, int>>> Handle(GetRideRequestByTimeQuery request, CancellationToken cancellationToken)
    {
           var rideRequests = await _unitOfWork.RideRequestRepository.GetAllByGivenParameter(request.RideRequestStatDto!.type,request.RideRequestStatDto.year, request.RideRequestStatDto.month);
            return new BaseResponse<Dictionary<int, int>>{
                Message = "Fetching Successful",
                Value = rideRequests
            };
    }
}
