using System;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class GetDriversStatisticsRequestHandler : IRequestHandler<GetDriversStatisticsRequest, BaseResponse<Dictionary<int, int>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDriversStatisticsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<BaseResponse<Dictionary<int, int>>> Handle(GetDriversStatisticsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Dictionary<int, int>>();

            var statistics = await _unitOfWork.DriverRepository.GetDriversStatistics(request.TimeFrame.ToLower(), request.Year, request.Month);

            response.Success = true;
            response.Message = "Fetch Succesful";
            response.Value = statistics;
            return response;
        }
    }
}