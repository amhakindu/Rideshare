using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
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

        var result = await _unitOfWork.RideRequestRepository.SearchByGivenParameter(request.PageNumber, request.PageSize, request.SearchAndFilterDto!.status, request.SearchAndFilterDto.fare, request.SearchAndFilterDto.name!, request.SearchAndFilterDto.phoneNumber!);
 
        response.Message = "Fetch Successful";
        response.Value = new PaginatedResponseDto<RideRequestDto>();

        response.Value.PageNumber = request.PageNumber;
        response.Value.PageSize = request.PageSize;
        response.Value.Count = result.Count;
        response.Value.Paginated = _mapper.Map<IReadOnlyList<RideRequestDto>>(result.Paginated);

        return response;
    }
}
