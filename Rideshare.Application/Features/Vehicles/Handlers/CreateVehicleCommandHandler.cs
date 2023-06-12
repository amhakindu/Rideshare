using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
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
            throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());

        Vehicle newVehicle = _mapper.Map<Vehicle>(request.VehicleDto);

        int operations = await _unitOfWork.VehicleRepository.Add(newVehicle);

        if (operations < 1)
        {
            throw new InternalServerErrorException("Unable to Save to Database");
        }

        return new BaseResponse<Nullable<int>>
        {
            Success = true,
            Message = "Vehicle Creation Success",
            Value = newVehicle.Id
        };
    }
}
