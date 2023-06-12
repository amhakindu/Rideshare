using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Handlers;
public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, BaseResponse<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleCommandHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<Unit>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateVehicleDtoValidator();
        var validationResult = await validator.ValidateAsync(request.VehicleDto);

        var vehicle = await _unitOfWork.VehicleRepository.Get(request.VehicleDto.Id);

        if (validationResult.IsValid == false || vehicle == null)
        {
            var response = new BaseResponse<Unit>
            {
                Success = false,
                Message = "Vehicle Update Failed"
            };
            if (vehicle == null)
            {
                var error = $"Vehicle with id={request.VehicleDto.Id} not found";
                response.Errors.Add(error);
            }
            else
            {
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            }
            return response;
        }

        vehicle.PlateNumber = request.VehicleDto.PlateNumber ?? vehicle.PlateNumber;
        vehicle.Libre = request.VehicleDto.Libre ?? vehicle.Libre;
        vehicle.UserId = request.VehicleDto.UserId ?? vehicle.UserId;

        int operations = await _unitOfWork.VehicleRepository.Update(vehicle);

        if (operations < 1)
        {
            throw new InternalServerErrorException("Unable to Save to Database");
        }

        return new BaseResponse<Unit>
        {
            Success = true,
            Message = "Vehicle Updated Successfully",
        };
    }
}
