using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetRideOffersQueryHandler: IRequestHandler<GetRideOffersQuery, BaseResponse<IReadOnlyList<RideOfferListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRideOffersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IReadOnlyList<RideOfferListDto>>> Handle(GetRideOffersQuery command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IReadOnlyList<RideOfferListDto>>();
            bool driverExists = CreateRideOfferDtoValidator.driverIDs.Exists(o => o == command.DriverID); // var driver = await _unitOfWork.DriverRepository.Get(command.DriverID);

            if (driverExists == false)
                throw new NotFoundException($"Driver with ID {command.DriverID} does not exist");
            var driverRideOffers = await _unitOfWork.RideOfferRepository.GetRideOffersOfDriver(command.DriverID, command.PageNumber, command.PageSize);
            var driverRideOfferDtos = _mapper.Map<IReadOnlyList<RideOffer>, IReadOnlyList<RideOfferListDto>>(driverRideOffers);

            return new BaseResponse<IReadOnlyList<RideOfferListDto>>{
                Success = true,
                Message = "Driver's RideOffers Fetching Successful",
                Value = driverRideOfferDtos
            };
        }
    }
}