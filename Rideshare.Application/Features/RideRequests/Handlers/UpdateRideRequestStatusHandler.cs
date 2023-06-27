using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideRequests.Commands;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class UpdateRideRequestStatusHandler : IRequestHandler<UpdateRideRequestStatusCommand, BaseResponse<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateRideRequestStatusHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<Unit>> Handle(UpdateRideRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<Unit>();
        var rideRequest = await _unitOfWork.RideRequestRepository.Get(request.Id);
        if (rideRequest != null && rideRequest.UserId == request.UserId){
            rideRequest.Status = Domain.Common.Status.CANCELED;
           var value =  await _unitOfWork.RideRequestRepository.Update(rideRequest);
                if ( value > 0)
                {
                    response.Message = "Updation Successful!";
                    response.Value =  new Unit();
                }
                else
                {
                    throw new InternalServerErrorException("Unable to update ride request!");
                }

        }else{
            throw new NotFoundException("Ride request not found");
        }

        return response;

    }
}

