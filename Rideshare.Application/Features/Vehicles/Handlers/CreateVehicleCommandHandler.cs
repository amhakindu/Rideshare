using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Handlers;
public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, BaseResponse<Nullable<int>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleCommandHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<int?>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateVehicleDtoValidator();
        var validationResult = await validator.ValidateAsync(request.VehicleDto);

        if (validationResult.IsValid == false)
        {
            return new BaseResponse<Nullable<int>>
            {
                Success = false,
                Message = "Vehicle Creation Failed",
                Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList()
            };
        }

        Vehicle newVehicle = _mapper.Map<Vehicle>(request.VehicleDto);

        int operations = await _unitOfWork.VehicleRepository.Add(newVehicle);

        if (operations > 0)
        {
            return new BaseResponse<Nullable<int>>
            {
                Success = false,
                Message = "Vehicle Creation Failed",
                Errors = new List<string>() { "Failed to save to database" }
            };
        }

        return new BaseResponse<Nullable<int>>
        {
            Success = true,
            Message = "Vehicle Creation Success",
            Value = newVehicle.Id
        };
    }
}
