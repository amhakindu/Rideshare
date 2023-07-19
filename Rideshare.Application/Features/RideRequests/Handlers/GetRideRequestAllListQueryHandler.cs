using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Queries;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestAllListQueryHandler : IRequestHandler<GetRideRequestAllListQuery,PaginatedResponse<RideRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRideRequestAllListQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<RideRequestDto>> Handle(GetRideRequestAllListQuery request, CancellationToken cancellationToken)
    {
        var response = new PaginatedResponse<RideRequestDto>();
        var result = await _unitOfWork.RideRequestRepository.GetAllRequests(request.PageNumber, request.PageSize);
        
        response.Message = "Get Successful";
        response.Value = _mapper.Map<IReadOnlyList<RideRequestDto>>(result.Value);
        response.Count = result.Count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        
        return response;
    }
}

