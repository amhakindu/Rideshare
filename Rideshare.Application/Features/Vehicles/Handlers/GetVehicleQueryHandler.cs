using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
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
public class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, BaseResponse<VehicleDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetVehicleQueryHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<VehicleDto>> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.VehicleRepository.Get(request.VehicleId);
        if (vehicle == null)
            throw new NotFoundException($"Vehicle with ID {request.VehicleId} does not exist");

        return new BaseResponse<VehicleDto>
        {
            Success = true,
            Message = "Vehicle Fetch Success",
            Value = _mapper.Map<VehicleDto>(vehicle)
        };
    }
}
