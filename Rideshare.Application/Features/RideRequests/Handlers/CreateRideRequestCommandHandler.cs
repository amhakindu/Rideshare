using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.RideRequests.Validators;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.RideRequests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Tests.Handlers;

public class CreateRideRequestCommandHandler : IRequestHandler<CreateRideRequestCommand, BaseResponse<Dictionary<string, object>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRideshareMatchingService _matchingService;
    private readonly IRideShareHubService _hubService;

    public  CreateRideRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IRideshareMatchingService matchingService, IRideShareHubService rideShareHubService)
    {
        _unitOfWork =  unitOfWork;
        _mapper = mapper;
        _matchingService = matchingService;
        _hubService = rideShareHubService;
    }
    public async Task<BaseResponse<Dictionary<string, object>>> Handle(CreateRideRequestCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<Dictionary<string, object>>();
        var validator = new CreateRideRequestDtoValidator();
        var validationResult = await validator.ValidateAsync(request.RideRequestDto);

        if (validationResult.IsValid == true){
            RideRequest rideRequest;
            try{
                rideRequest = _mapper.Map<RideRequest>(request.RideRequestDto);
            }catch{
                throw new ValidationException("No Valid Address Found");
            }
            var value =  await _unitOfWork.RideRequestRepository.Add(rideRequest);
            if (value > 0)
            {
                var matchedRideOffer = await _matchingService.MatchWithRideoffer(rideRequest);
                if(matchedRideOffer == null)
                    throw new ValidationException($"No RideOffer Found That Can Complete This Request. Try Again Later");
                response.Message = "Creation Successful";
                response.Value = new Dictionary<string, object>{
                    {"Id", rideRequest.Id},
                    {"MatchedRide", _mapper.Map<RideOfferDto>(rideRequest.MatchedRide)}
                };

                var userId = matchedRideOffer.Driver.UserId;
                var rideRequestDto = _mapper.Map<RideRequestDto>(rideRequest);
                _hubService.MatchFound(userId, rideRequestDto);
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