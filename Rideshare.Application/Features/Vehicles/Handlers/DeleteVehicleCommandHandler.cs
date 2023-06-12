using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var Vehicle = await _unitOfWork.VehicleRepository.Get(request.VehicleId);
        if (Vehicle == null)
        {
            var error = $"Vehicle with id={request.VehicleId} was not found";
            var response = new BaseResponse<Unit>
            {
                Success = false,
                Message = "Vehicle Deletion Failed",
            };
            response.Errors.Add(error);
            return response;
        }
        var operations = await _unitOfWork.VehicleRepository.Delete(Vehicle);

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
