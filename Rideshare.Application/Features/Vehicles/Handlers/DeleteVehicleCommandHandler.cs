using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Rideshare.Application.Features.Vehicles.Handlers;
public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, BaseResponse<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleCommandHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<Unit>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.VehicleRepository.Get(request.VehicleId);
        if (vehicle == null)
            throw new NotFoundException($"Vehicle with ID {request.VehicleId} does not exist");

        var operations = await _unitOfWork.VehicleRepository.Delete(vehicle);

        if (operations < 1)
        {
            throw new InternalServerErrorException("Unable to Save to Database");
        }

        return new BaseResponse<Unit>()
        {
            Success = true,
            Message = "Vehicle Deleted Successfully",
        };
    }
}
