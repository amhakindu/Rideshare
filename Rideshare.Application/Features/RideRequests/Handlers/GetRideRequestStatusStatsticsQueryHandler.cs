using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestStatusStatsticsQueryHandler : IRequestHandler<GetRideRequestStatusStatsticsQuery, BaseResponse<Dictionary<string, Dictionary<int, int>>>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRideRequestStatusStatsticsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Dictionary<string, Dictionary<int, int>>>> Handle(GetRideRequestStatusStatsticsQuery request, CancellationToken cancellationToken)
    {
            var rideRequests = await _unitOfWork.RideRequestRepository.GetAllByGivenStatus(request.option,request.Year, request.Month);
            return new BaseResponse<Dictionary<string, Dictionary<int, int>>>{

                Message = "Fetching Successful",
                Value = rideRequests
            };
    }
}
