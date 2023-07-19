using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;

namespace Rideshare.Application.Features.Drivers.Handlers;

public class GetDriversListRequestHandler : IRequestHandler<GetDriversListRequest, PaginatedResponse<DriverDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDriversListRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;

    }
    public async Task<PaginatedResponse<DriverDetailDto>> Handle(GetDriversListRequest request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DriverRepository.GetDriversWithDetails(request.PageNumber, request.PageSize);

        return new PaginatedResponse<DriverDetailDto>(){
            Message= "Fetch Successful",
            Value= _mapper.Map<IReadOnlyList<Driver>, IReadOnlyList<DriverDetailDto>>(result.Value),
            Count= result.Count,
            PageNumber= request.PageNumber,
            PageSize= request.PageSize
        };
    }
}

