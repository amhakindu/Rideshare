using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestListQueryHandler : IRequestHandler<GetRideRequestListQuery, BaseResponse<PaginatedResponseDto<RideRequestDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public GetRideRequestListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    public async Task<BaseResponse<PaginatedResponseDto<RideRequestDto>>> Handle(GetRideRequestListQuery request, CancellationToken cancellationToken)
    {

        var response = new BaseResponse<PaginatedResponseDto<RideRequestDto>>();


        var result = await _unitOfWork.RideRequestRepository.GetAll(request.PageNumber, request.PageSize);

        List<RideRequest> rideReqs = new List<RideRequest>();
        foreach (var rideRequest in result.Paginated)
        {
            if (rideRequest.UserId == (request.UserId ?? rideRequest.UserId))
            {
                rideReqs.Add(rideRequest);
            }
        }

        var rides = _mapper.Map<List<RideRequest>, List<RideRequestDto>>(rideReqs);
        response.Message = "Fetch Successful";
        response.Value = new PaginatedResponseDto<RideRequestDto>();
        response.Value.PageNumber = request.PageNumber;
        response.Value.PageSize = request.PageSize;
        response.Value.Paginated = rides;

        return response;
    }
}
