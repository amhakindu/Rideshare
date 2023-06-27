using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Common.Dtos.Vehicles;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetRideOffersOfDriverQueryHandler: IRequestHandler<GetRideOffersOfDriverQuery, BaseResponse<Dictionary<string, object>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public GetRideOffersOfDriverQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IMapboxService mapboxService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapboxService = mapboxService;
        }

        public async Task<BaseResponse<Dictionary<string, object>>> Handle(GetRideOffersOfDriverQuery command, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(command.UserId);
            if (driver == null)
                throw new NotFoundException($"User with ID {command.UserId} is not a driver");
            
            var driverRideOffers = await _unitOfWork.RideOfferRepository.GetRideOffersOfDriver(driver.Id, command.PageNumber, command.PageSize);

            return new BaseResponse<Dictionary<string, object>>{
                Success = true,
                Message = "RideOffers Fetching Successful",
                Value = new Dictionary<string, object>(){
                    {"count", driverRideOffers["count"]},
                    {"rideoffers", _mapper.Map<IReadOnlyList<RideOffer>, IReadOnlyList<RideOfferListDto>>((IReadOnlyList<RideOffer>)driverRideOffers["rideoffers"])}
                }
            };
        }
    }
}