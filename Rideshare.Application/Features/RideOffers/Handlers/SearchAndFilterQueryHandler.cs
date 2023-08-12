using MediatR;
using AutoMapper;
using Rideshare.Domain.Common;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Common.Dtos.RideOffers.Validators;


namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers;

public class SearchAndFilterQueryHandler: IRequestHandler<SearchAndFilterQuery, PaginatedResponse<RideOfferListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchAndFilterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<RideOfferListDto>> Handle(SearchAndFilterQuery command, CancellationToken cancellationToken)
    {
        var response = new PaginatedResponse<RideOfferListDto>();
        var validator = new RideOffersListFilterDtoValidator();
        var validationResult = await validator.ValidateAsync(command.SearchDto);
        if (validationResult.IsValid == false)
            throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());
        Status? status = null;
        if(command.SearchDto.Status != null)
            status = (Status)Enum.Parse(typeof(Status), command.SearchDto.Status);
        var result = await _unitOfWork.RideOfferRepository.SearchAndFilter(command.SearchDto.MinCost, command.SearchDto.MaxCost, command.SearchDto.DriverName, command.SearchDto.PhoneNumber, status, command.PageNumber, command.PageSize);

        response.Success = true;
        response.Message = "RideOffers Fetching Successful";
        response.Value = _mapper.Map<IReadOnlyList<RideOfferListDto>>(result.Value);
        response.Count = result.Count;
        response.PageNumber = command.PageNumber;
        response.PageSize = command.PageSize;
        return response;        
    }
}