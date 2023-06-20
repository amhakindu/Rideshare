using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Vehicles.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Vehicles.Handlers;
public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, BaseResponse<Nullable<int>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResourceManager _resourceManager;

    public CreateVehicleCommandHandler(IMapper mapper, IUnitOfWork work, IResourceManager resourceManager)
    {
        _mapper = mapper;
        _unitOfWork = work;
        _resourceManager = resourceManager;
    }

    public async Task<BaseResponse<int?>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateVehicleDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.VehicleDto);

        if (validationResult.IsValid == false)
            throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());

        Vehicle newVehicle = _mapper.Map<Vehicle>(request.VehicleDto);
        var uri = await _resourceManager.UploadPDF(request.VehicleDto.Libre);
        newVehicle.Libre = uri.AbsoluteUri;

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
