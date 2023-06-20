using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests.Validators;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Handlers;

 
public class UpdateRideRequestCommandHandler : IRequestHandler<UpdateRideRequestCommand, BaseResponse<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public   UpdateRideRequestCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        
    }

    public async Task<BaseResponse<Unit>> Handle(UpdateRideRequestCommand request, CancellationToken cancellationToken)
    {

        var response = new BaseResponse<Unit>();
        var validator = new UpdateRideRequestDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.RideRequestDto);
 
        if (validationResult.IsValid == true){
            var rideRequest = await _unitOfWork.RideRequestRepository.Get(request.RideRequestDto.Id);
           rideRequest =  _mapper.Map(request.RideRequestDto, rideRequest);

           if (rideRequest.UserId == request.UserId){
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

           }else {
            throw new NotFoundException("Unable to get this ride request");
           }
        }
        else{
             throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());
        }

        return response;
    }
}
