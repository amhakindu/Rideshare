using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideRequests.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Handlers;

public class DeleteRideRequestCommandHandler : IRequestHandler<DeleteRideRequestCommand,BaseResponse<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public     DeleteRideRequestCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
         _unitOfWork= unitOfWork;
        _mapper = mapper;
        
    }

    public async Task<BaseResponse<Unit>> Handle(DeleteRideRequestCommand request, CancellationToken cancellationToken)
    { 

        var response = new BaseResponse<Unit>();
        
 
            var  rideRequest = await _unitOfWork.RideRequestRepository.Get(request.Id);
            if (rideRequest == null || rideRequest.UserId != request.UserId){
                 throw new NotFoundException("RideRequest not found");
            }
            else{

            var value = await _unitOfWork.RideRequestRepository.Delete(rideRequest);

            if (value == request.Id)
            {
                
                response.Message = "Deletion Successful!";
                response.Value = new Unit();
            }
            else
            {
                throw new InternalServerErrorException("Deletion Failed");
            }
        } 
        return response;
         
    }
} 


