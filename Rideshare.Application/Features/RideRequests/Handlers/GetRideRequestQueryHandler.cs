using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestQueryHandler : IRequestHandler<GetRideRequestQuery, BaseResponse<RideRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper  ;
    

    public GetRideRequestQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        
    }
    public async Task<BaseResponse<RideRequestDto>> Handle(GetRideRequestQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<RideRequestDto>();
        var rideRequest = await _unitOfWork.RideRequestRepository.Get(request.Id);
        if (rideRequest != null && rideRequest.UserId == request.UserId){
            response.Message = "Get Successful";
            response.Value = _mapper.Map<RideRequestDto>(rideRequest);
        }
        else{
            throw new NotFoundException($"RideRequest with {request.Id} not found");
        }
        return response;
    }
}
