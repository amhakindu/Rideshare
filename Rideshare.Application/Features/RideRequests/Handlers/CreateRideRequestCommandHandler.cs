using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.RideRequests.Validators;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Tests.Handlers;

public class CreateRideRequestCommandHandler : IRequestHandler<CreateRideRequestCommand, BaseResponse<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

     public  CreateRideRequestCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork =  unitOfWork;
        _mapper = mapper;
        
    }
    public async Task<BaseResponse<int>> Handle(CreateRideRequestCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();
            var validator = new CreateRideRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.RideRequestDto);

             
        if (validationResult.IsValid == true){
            var rideRequest = _mapper.Map<RideRequest>(request.RideRequestDto);
            var value =  await _unitOfWork.RideRequestRepository.Add(rideRequest);
            if (value > 0)
            {
               
                response.Message = "Creation Successful";
                response.Value = rideRequest.Id ;
            }
            else
            {
                throw new InternalServerErrorException($"{value}Unable to create the ride request");
            }
        }
        else
        {
           throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());
        }
      
        return response;
        
    }
}
