using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Features.RideOffers.Queries;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers;

public class GetRideOffersOfDriverQueryHandler: IRequestHandler<GetRideOffersOfDriverQuery, PaginatedResponse<RideOfferListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRideOffersOfDriverQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<RideOfferListDto>> Handle(GetRideOffersOfDriverQuery command, CancellationToken cancellationToken)
    {

        var response = new PaginatedResponse<RideOfferListDto>();
        int driverId = 0;
        if(command.Id.GetType() == " ".GetType()){
            var driver = await _unitOfWork.DriverRepository.GetDriverByUserId((string)command.Id);
            if (driver != null)
                driverId = driver.Id;
        }else{
            if(await _unitOfWork.DriverRepository.Exists((int)command.Id))
                driverId = (int)command.Id;
        }
        
        var result = await _unitOfWork.RideOfferRepository.GetRideOffersOfDriver(driverId, command.PageNumber, command.PageSize);
        
        response.Success = true;
        response.Message = "RideOffers Fetching Successful";
        response.Value = _mapper.Map<IReadOnlyList<RideOfferListDto>>(result.Value);
        response.Count = result.Count;
        response.PageNumber = command.PageNumber;
        response.PageSize = command.PageSize;
        return response;
        
    }
}