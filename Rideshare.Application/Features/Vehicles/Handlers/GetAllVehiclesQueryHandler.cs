using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Vehicles.Queries;

namespace Rideshare.Application.Features.Vehicles.Handlers;

public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, PaginatedResponse<VehicleDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllVehiclesQueryHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<PaginatedResponse<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var response = new PaginatedResponse<VehicleDto>();
        var result = await _unitOfWork.VehicleRepository.GetAll(request.PageNumber, request.PageSize);

        var vehicleDtos = result.Value.Select(vehicle => _mapper.Map<VehicleDto>(vehicle)).ToList();
        response.Success = true;
        response.Message = "Vehicles Fetching Successful";
        response.Value = vehicleDtos;
        response.Count = result.Count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;

        return response;
    }
}
