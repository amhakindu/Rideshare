using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestListQueryHandler : IRequestHandler<GetRideRequestListQuery, BaseResponse<Dictionary<string,object>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    
    public GetRideRequestListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;  
        
    }
    public async Task<BaseResponse<Dictionary<string,object>>> Handle(GetRideRequestListQuery request, CancellationToken cancellationToken)
    {

        var response = new BaseResponse<Dictionary<string,object>>();
        var rideRequests = await _unitOfWork.RideRequestRepository.SearchByGivenParameter(request.PageNumber, request.PageSize,request.SearchAndFilterDto!.status,request.SearchAndFilterDto.fare,request.SearchAndFilterDto.name!,request.SearchAndFilterDto.phoneNumber!);
         
        response.Message = "Get Successful";
        response.Value = new Dictionary<string, object>(){
                    {"count", rideRequests["count"]},
                    {"riderequests", _mapper.Map<IReadOnlyList<RideRequest>, IReadOnlyList<RideRequestDto>>((IReadOnlyList<RideRequest>)rideRequests["riderequests"])}};
        return response;
    }
}
