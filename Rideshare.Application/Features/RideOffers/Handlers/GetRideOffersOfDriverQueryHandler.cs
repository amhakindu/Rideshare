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
using Rideshare.Application.Common.Dtos.Pagination;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetRideOffersOfDriverQueryHandler: IRequestHandler<GetRideOffersOfDriverQuery, BaseResponse<PaginatedResponseDto<RideOfferDto>>>
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

        public async Task<BaseResponse<PaginatedResponseDto<RideOfferDto>>> Handle(GetRideOffersOfDriverQuery command, CancellationToken cancellationToken)
        {

            var response = new BaseResponse<PaginatedResponseDto<RideOfferDto>>();
        
            var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(command.UserId);
            if (driver == null)
                throw new NotFoundException($"User with ID {command.UserId} is not a driver");
            
            var result = await _unitOfWork.RideOfferRepository.GetRideOffersOfDriver(driver.Id, command.PageNumber, command.PageSize);
            

            response.Success = true;
            response.Message = "RideOffers Fetching Successful";
            response.Value = new PaginatedResponseDto<RideOfferDto>();
            response.Value.Count = result.Count;
            response.Value.Paginated = _mapper.Map<IReadOnlyList<RideOfferDto>>(result.Paginated);
            return response;
            
        }
    }
}