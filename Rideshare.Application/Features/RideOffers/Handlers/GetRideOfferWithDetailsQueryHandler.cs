using System.Threading.Tasks.Dataflow;
using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.Movies.CQRS.Handlers
{
    public class GetRideOfferWithDetailsQueryHandler : IRequestHandler<GetRideOfferWithDetailsQuery, BaseResponse<RideOfferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRideOfferWithDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<RideOfferDto>> Handle(GetRideOfferWithDetailsQuery query, CancellationToken cancellationToken)
        {
            var rideOffer = await _unitOfWork.RideOfferRepository.Get(query.RideOfferID);
            
            if(rideOffer == null)
                throw new NotFoundException($"RideOffer with ID {query.RideOfferID} does not exist");

            return new BaseResponse<RideOfferDto>{
                Success=true,
                Message="RideOffer Fetching Successful",
                Value=_mapper.Map<RideOfferDto>(rideOffer)
            };
        }
    }
}
