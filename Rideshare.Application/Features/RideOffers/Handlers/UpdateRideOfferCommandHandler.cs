using MediatR;
using AutoMapper;
using System.Data;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;
using NetTopologySuite.Geometries;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class UpdateRideOfferCommandHandler: IRequestHandler<UpdateRideOfferCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRideOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Unit>> Handle(UpdateRideOfferCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Unit>();
            var rideOffer = await _unitOfWork.RideOfferRepository.Get(command.RideOfferDto.Id);

            if (rideOffer == null)
                throw new NotFoundException($"RideOffer with ID {command.RideOfferDto.Id} does not exist");

            Vehicle? vehicle = null;
            if(command.RideOfferDto.VehicleID != null){
                vehicle = await _unitOfWork.VehicleRepository.Get((int)command.RideOfferDto.VehicleID);
                if(vehicle == null)
                    throw new NotFoundException($"Vehicle with ID {command.RideOfferDto.VehicleID} does not exist");
            }

            var validator = new UpdateRideOfferDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(command.RideOfferDto);

            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());

            rideOffer.Vehicle = vehicle ?? rideOffer.Vehicle;
            rideOffer.CurrentLocation = _mapper.Map<GeographicalLocation>(command.RideOfferDto.CurrentLocation) ?? rideOffer.CurrentLocation;
            rideOffer.Destination = _mapper.Map<GeographicalLocation>(command.RideOfferDto.Destination) ?? rideOffer.Destination;
            if(command.RideOfferDto.Status != null)
                rideOffer.Status = (Status)Enum.Parse(typeof(Status), command.RideOfferDto.Status);
                
            
            var numOfOperations = await _unitOfWork.RideOfferRepository.Update(rideOffer);
            if (numOfOperations == 0)
                throw new InternalServerErrorException($"Unable to Save to Database");

            return new BaseResponse<Unit>{
                Success=true,
                Message="RideOffer Updated Successfully",
                Value=Unit.Value
            };
        }
    }
}
