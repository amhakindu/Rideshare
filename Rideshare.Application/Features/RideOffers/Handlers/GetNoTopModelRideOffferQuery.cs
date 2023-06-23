using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.RideOffers.Handlers
{
    public class GetNoTopModelRideOffferQueryHandler : IRequestHandler<GetNoTopModelRideOffferQuery, BaseResponse<IReadOnlyList<ModelAndCountDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNoTopModelRideOffferQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IReadOnlyList<ModelAndCountDto>>> Handle(GetNoTopModelRideOffferQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IReadOnlyList<ModelAndCountDto>>();
            var result = await _unitOfWork.RideOfferRepository.NoTopModelOffers();
            response.Value = result;
            response.Message = "Number of rideoffers for top 10 models are succesfully fetched";
            return response;
        }
    }
}
