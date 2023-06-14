using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestListQueryHandler : IRequestHandler<GetRideRequestListQuery, BaseResponse<List<RideRequestDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRideRequestListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;  
    }
    public async Task<BaseResponse<List<RideRequestDto>>> Handle(GetRideRequestListQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<RideRequestDto>>();

        var rideRequests = (List<RideRequest>)await _unitOfWork.RideRequestRepository.GetAll();

        var  rides = _mapper.Map<List<RideRequest>, List<RideRequestDto>>(rideRequests);
        response.Message = "Get Successful";
        response.Value = rides;

        return response;
    }
}
