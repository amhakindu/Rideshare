using System.Data;
using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Infrastructure;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class CreateRideOfferCommandHandler: IRequestHandler<CreateRideOfferCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateRideOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<int>> Handle(CreateRideOfferCommand command, CancellationToken cancellationToken)
        {
            
            if(!await _unitOfWork.VehicleRepository.Exists(command.RideOfferDto.VehicleID))
                throw new NotFoundException($"Vehicle with ID {command.RideOfferDto.VehicleID} does not exist");
            var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(command.UserId);

            if(await _unitOfWork.RideOfferRepository.GetActiveRideOfferOfDriver(driver.Id) != null){
                return new BaseResponse<int>(){
                    Success=false,
                    Message="A Driver Can Only Provide One RideOffer At a Time",
                    Value = 0,
                };
            }

            var validator = new CreateRideOfferDtoValidator();
            var validationResult = await validator.ValidateAsync(command.RideOfferDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());

            var rideOffer = _mapper.Map<RideOffer>(command.RideOfferDto);

            if(rideOffer.EstimatedFare == 0 && rideOffer.EstimatedDuration.TotalSeconds == 0){
                return new BaseResponse<int>(){
                    Success=false,
                    Message="No Route Found For The Given Origin and Destination",
                    Value = 0
                };
            }

            var dbOperations = await _unitOfWork.RideOfferRepository.Add(rideOffer);

            if (dbOperations == 0)
                throw new InternalServerErrorException("Unable to Save to Database");

            return new BaseResponse<int>{
                Success=true,
                Message="RideOffer Created Successfully",
                Value=rideOffer.Id
            };
        }
    }
}
