using System;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class GetDriversStatisticsRequestHandler : IRequestHandler<GetDriversStatisticsRequest, BaseResponse<Dictionary<string, int>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDriversStatisticsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<BaseResponse<Dictionary<string, int>>> Handle(GetDriversStatisticsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Dictionary<string, int>>();

            var statistics = await _unitOfWork.DriverRepository.GetDriversStatistics(request.Weekly, request.Monthly, request.Yearly);

            response.Success = true;
            response.Message = "Fetch Succesful";
            response.Value = statistics;
            return response;
        }
    }
}