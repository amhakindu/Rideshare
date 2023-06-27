using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetRideOfferStatsWithStatusQueryHandler: IRequestHandler<GetRideOfferStatsWithStatusQuery, BaseResponse<Dictionary<string, Dictionary<int, int>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public GetRideOfferStatsWithStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dictionary<string, Dictionary<int, int>>>> Handle(GetRideOfferStatsWithStatusQuery command, CancellationToken cancellationToken)
        {
            var validator = new RideOfferStatsDtoValidator();
            var validationResult = await validator.ValidateAsync(command.StatsDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());

            var rideOfferStats = await _unitOfWork.RideOfferRepository.GetRideOfferStatisticsWithStatus(command.StatsDto.Year, command.StatsDto.Month);

            return new BaseResponse<Dictionary<string, Dictionary<int, int>>>{
                Success = true,
                Message = "RideOffers Statistics Fetching Successful",
                Value = rideOfferStats
            };
        }
    }
}