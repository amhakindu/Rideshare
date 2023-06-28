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
            if(driver == null)
                throw new OperationFailure($"Only A Driver Can Create A RideOffer");

            if(await _unitOfWork.RideOfferRepository.GetActiveRideOfferOfDriver(driver.Id) != null)
                throw new OperationFailure("A Driver Can Only Provide One RideOffer At a Time");

            var validator = new CreateRideOfferDtoValidator();
            var validationResult = await validator.ValidateAsync(command.RideOfferDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());

            var rideOffer = _mapper.Map<RideOffer>(command.RideOfferDto);

            if(rideOffer.EstimatedFare == 0 && rideOffer.EstimatedDuration.TotalSeconds == 0)
                throw new ValidationException("No Route Found For The Given Origin and Destination");

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
