using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Commands;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class DeleteRideOfferCommandHandler: IRequestHandler<DeleteRideOfferCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteRideOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Unit>> Handle(DeleteRideOfferCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Unit>();
            var rideOffer = await _unitOfWork.RideOfferRepository.Get(command.RideOfferID);

            if (rideOffer == null)
                throw new NotFoundException($"RideOffer with ID {command.RideOfferID} does not exist");
            var dbOperations = await _unitOfWork.RideOfferRepository.Delete(rideOffer);

            if (dbOperations == 0)
                throw new InternalServerErrorException($"Unable to Save to Database");

            return new BaseResponse<Unit>{
                Success = true,
                Message = "RideOffer Deletion Successful",
                Value = Unit.Value,
                Errors = new List<string>{"Unable to save to database"}
            };
        }
    }
}
