using MediatR;
using AutoMapper;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Common.Dtos.Pagination;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetAllRideOffersQueryHandler: IRequestHandler<GetAllRideOffersQuery, BaseResponse<PaginatedResponseDto<RideOfferDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public GetAllRideOffersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IMapboxService mapboxService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapboxService = mapboxService;
        }

        public async Task<BaseResponse<PaginatedResponseDto<RideOfferDto>>> Handle(GetAllRideOffersQuery command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResponseDto<RideOfferDto>>();
            var result= await _unitOfWork.RideOfferRepository.GetAllPaginated(command.PageNumber, command.PageSize);

            response.Success = true;
            response.Message = "RideOffers Fetching Successful";
            response.Value = new PaginatedResponseDto<RideOfferDto>();
            response.Value.Count = result.Count;
            response.Value.PageNumber = command.PageNumber;
            response.Value.PageSize = command.PageSize;
            response.Value.Paginated = _mapper.Map<IReadOnlyList<RideOfferDto>>(result.Paginated);

            return response;
            
        }
    }
}