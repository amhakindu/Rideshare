using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;

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
           var history = await _unitOfWork.RideRequestRepository.GetEntityStatistics(request.Year, request.Month);
            return new BaseResponse<Dictionary<int, int>>{
                Message = "Fetching Successful",
                Value = history
            };
    }
}
