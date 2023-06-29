using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class GetRideRequestUserQueryHandler : IRequestHandler<GetRideRequestUserQuery, BaseResponse<PaginatedResponseDto<RideRequestDto>>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public GetRideRequestUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<BaseResponse<PaginatedResponseDto<RideRequestDto>>> Handle(GetRideRequestUserQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<PaginatedResponseDto<RideRequestDto>>();


        var result = await _unitOfWork.RideRequestRepository.GetAllUserRequests(request.PageNumber, request.PageSize, request.UserId!);


        response.Message = "Get Successful";

        response.Value = new PaginatedResponseDto<RideRequestDto>();
        response.Value.Count = result.Count;
        response.Value.Paginated = _mapper.Map<IReadOnlyList<RideRequestDto>>(result.Paginated);
        response.Value.PageNumber = request.PageNumber;
        response.Value.PageSize = request.PageSize;


        return response;
    }
}
