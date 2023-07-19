using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Queries;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestListQueryHandler : IRequestHandler<GetRideRequestListQuery, PaginatedResponse<RideRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRideRequestListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<RideRequestDto>> Handle(GetRideRequestListQuery request, CancellationToken cancellationToken)
    { 

        var response = new PaginatedResponse<RideRequestDto>();

        var result = await _unitOfWork.RideRequestRepository.SearchByGivenParameter(request.PageNumber, request.PageSize, request.RideRequestsListFilterDto!.status, request.RideRequestsListFilterDto.fare, request.RideRequestsListFilterDto.name!, request.RideRequestsListFilterDto.phoneNumber!);
 
        response.Message = "Fetch Successful";
        response.Value = _mapper.Map<IReadOnlyList<RideRequestDto>>(result.Value);
        response.Count = result.Count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;

        return response;
    }
}
