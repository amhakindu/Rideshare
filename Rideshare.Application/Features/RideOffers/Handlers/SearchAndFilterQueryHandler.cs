using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;
using Rideshare.Application.Exceptions;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class SearchAndFilterQueryHandler: IRequestHandler<SearchAndFilterQuery, BaseResponse<Dictionary<string, object>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public SearchAndFilterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dictionary<string, object>>> Handle(SearchAndFilterQuery command, CancellationToken cancellationToken)
        {
            var validator = new SearchAndFilterDtoValidator();
            var validationResult = await validator.ValidateAsync(command.SearchDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());
            Status? status = null;
            if(command.SearchDto.Status != null)
                status = (Status)Enum.Parse(typeof(Status), command.SearchDto.Status);
            var driverRideOffers = await _unitOfWork.RideOfferRepository.SearchAndFilter(command.SearchDto.MinCost, command.SearchDto.MaxCost, command.SearchDto.DriverName, command.SearchDto.PhoneNumber, status, command.PageNumber, command.PageSize);

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