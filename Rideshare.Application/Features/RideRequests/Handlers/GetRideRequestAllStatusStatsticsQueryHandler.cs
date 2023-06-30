using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestAllStatusStatsticsQueryHandler : IRequestHandler<GetRideRequestAllStatusStatsticsQuery, BaseResponse<Dictionary<string, int>>>
{


    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRideRequestAllStatusStatsticsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    
    public async Task<BaseResponse<Dictionary<string, int>>> Handle(GetRideRequestAllStatusStatsticsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<Dictionary<string,int>>();
        var result = await _unitOfWork.RideRequestRepository.GetStatusStatistics();
        response.Message = "Fetch Successful";
        response.Value = result;
        return response;

    }
}
