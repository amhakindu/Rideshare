using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Exceptions;


namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class CancelRideOfferCommandHandler: IRequestHandler<CancelRideOfferCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CancelRideOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Unit>> Handle(CancelRideOfferCommand command, CancellationToken cancellationToken)
        {
            var rideOffer = await _unitOfWork.RideOfferRepository.Get(command.RideOfferId);

            if (rideOffer == null)
                throw new NotFoundException($"RideOffer with ID {command.RideOfferId} does not exist");

            var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(command.UserId);
            if(driver == null || driver.Id != rideOffer.Driver.Id)
                throw new NotAllowedException("Only Owners Can Cancel Their RideOffers");

            var dbOperations = await _unitOfWork.RideOfferRepository.CancelRideOffer(command.RideOfferId);

            if (dbOperations == 0)
                throw new InternalServerErrorException($"Unable to Save to Database");

            return new BaseResponse<Unit>{
                Success = true,
                Message = "RideOffer Cancellation Successful",
                Value = Unit.Value,
            };
        }
    }
}
