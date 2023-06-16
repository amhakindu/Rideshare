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

        if (validationResult.IsValid == false)
            throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());


        var vehicle = await _unitOfWork.VehicleRepository.Get(request.VehicleDto.Id);
        if (vehicle == null)
            throw new NotFoundException($"Vehicle with ID {request.VehicleDto.Id} does not exist");
        

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
