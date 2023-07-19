using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Features.RideOffers.Queries;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers;

public class GetAllRideOffersQueryHandler: IRequestHandler<GetAllRideOffersQuery, PaginatedResponse<RideOfferListDto>>
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

    public async Task<PaginatedResponse<RideOfferListDto>> Handle(GetAllRideOffersQuery command, CancellationToken cancellationToken)
    {
        var response = new PaginatedResponse<RideOfferListDto>();
        var result= await _unitOfWork.RideOfferRepository.GetAllPaginated(command.PageNumber, command.PageSize);

        response.Success = true;
        response.Message = "RideOffers Fetching Successful";
        response.Value = _mapper.Map<IReadOnlyList<RideOfferListDto>>(result.Value);
        response.Count = result.Count;
        response.PageNumber = command.PageNumber;
        response.PageSize = command.PageSize;

        return response;
        
    }
}