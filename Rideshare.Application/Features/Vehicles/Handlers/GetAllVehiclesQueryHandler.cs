using AutoMapper;
using MediatR;
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
public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, BaseResponse<IList<VehicleDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllVehiclesQueryHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<IList<VehicleDto>>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<Vehicle> vehicles = await _unitOfWork.VehicleRepository.GetAll();

        var vehicleDtos = vehicles.Select(vehicle => _mapper.Map<VehicleDto>(vehicle)).ToList();
        return new BaseResponse<IList<VehicleDto>>()
        {
            Success = true,
            Value = vehicleDtos
        };
    }
}
