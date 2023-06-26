using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
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
    public class GetTopDriversWithStatsRequestHandler : IRequestHandler<GetTopDriversWithStatsRequest, BaseResponse<List<DriverStatsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTopDriversWithStatsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<BaseResponse<List<DriverStatsDto>>> Handle(GetTopDriversWithStatsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<DriverStatsDto>>();



            var stats = await _unitOfWork.RideOfferRepository.GetTopDriversWithStats();

            response.Success = true;
            response.Message = "Fetch Succesful";
            response.Value = stats;

            return response;
        }
    }
}
