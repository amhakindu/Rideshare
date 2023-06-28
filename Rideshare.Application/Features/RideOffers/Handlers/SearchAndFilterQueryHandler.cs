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
using Rideshare.Application.Common.Dtos.Pagination;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class SearchAndFilterQueryHandler: IRequestHandler<SearchAndFilterQuery, BaseResponse<PaginatedResponseDto<RideOfferDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public SearchAndFilterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<PaginatedResponseDto<RideOfferDto>>> Handle(SearchAndFilterQuery command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResponseDto<RideOfferDto>>();
            var validator = new SearchAndFilterDtoValidator();
            var validationResult = await validator.ValidateAsync(command.SearchDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());
            Status? status = null;
            if(command.SearchDto.Status != null)
                status = (Status)Enum.Parse(typeof(Status), command.SearchDto.Status);
            var result = await _unitOfWork.RideOfferRepository.SearchAndFilter(command.SearchDto.MinCost, command.SearchDto.MaxCost, command.SearchDto.DriverName, command.SearchDto.PhoneNumber, status, command.PageNumber, command.PageSize);

            response.Success = true;
            response.Message = "RideOffers Fetching Successful";
            response.Value= new PaginatedResponseDto<RideOfferDto>();
            response.Value.PageNumber = command.PageNumber;
            response.Value.PageSize = command.PageSize;
            response.Value.Paginated = _mapper.Map<IReadOnlyList<RideOfferDto>>(result.Paginated);
            response.Value.Count = result.Count;
            return response;
            
        }
    }
}