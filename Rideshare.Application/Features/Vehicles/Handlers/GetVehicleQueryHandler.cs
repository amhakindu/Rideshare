using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Responses;
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
        bool exists = await _unitOfWork.VehicleRepository.Exists(request.VehicleId);
        if (exists == false)
        {
            var error = $"Vehicle with id={request.VehicleId} was not found";
            return new BaseResponse<VehicleDto>
            {
                Success = false,
                Message = "Vehicle Fetch Failed",
                Errors = new List<string>() { error }
            };
        }
        var vehicle = await _unitOfWork.VehicleRepository.Get(request.VehicleId);
        return new BaseResponse<VehicleDto>
        {
            Success = true,
            Message = "Vehicle Fetch Success",
            Value = _mapper.Map<VehicleDto>(vehicle)
        };
    }
}
