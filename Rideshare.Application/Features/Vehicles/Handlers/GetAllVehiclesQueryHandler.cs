using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Handlers;
public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, BaseResponse<PaginatedResponseDto<VehicleDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllVehiclesQueryHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<PaginatedResponseDto<VehicleDto>>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<PaginatedResponseDto<VehicleDto>>();
        var result = await _unitOfWork.VehicleRepository.GetAll(request.PageNumber, request.PageSize);

        var vehicleDtos = result.Paginated.Select(vehicle => _mapper.Map<VehicleDto>(vehicle)).ToList();
        response.Success = true;
        response.Value = new PaginatedResponseDto<VehicleDto>();
        response.Value.Count = result.Count;
        response.Value.Paginated = vehicleDtos;
        response.Value.PageNumber = request.PageNumber;
        response.Value.PageSize = request.PageSize;

        return response;
    }
}
