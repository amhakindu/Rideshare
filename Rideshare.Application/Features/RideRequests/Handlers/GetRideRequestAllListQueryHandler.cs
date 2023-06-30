using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestAllListQueryHandler : IRequestHandler<GetRideRequestAllListQuery,BaseResponse<PaginatedResponseDto<RideRequestDto>>>
{
 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetRideRequestAllListQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<PaginatedResponseDto<RideRequestDto>>> Handle(GetRideRequestAllListQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<PaginatedResponseDto<RideRequestDto>>();
        var result = await _unitOfWork.RideRequestRepository.GetAllRequests(request.PageNumber, request.PageSize);

        
        response.Message = "Get Successful";

        response.Value = new PaginatedResponseDto<RideRequestDto>();
        response.Value.Count = result.Count;
        response.Value.Paginated = _mapper.Map<IReadOnlyList<RideRequestDto>>(result.Paginated);
        response.Value.PageNumber = request.PageNumber;
        response.Value.PageSize = request.PageSize;
        

        return response;
    }
    }

