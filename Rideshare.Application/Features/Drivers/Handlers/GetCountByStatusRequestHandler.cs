using System;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class GetCountByStatusRequestHandler : IRequestHandler<GetCountByStatusRequest, BaseResponse<List<int>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCountByStatusRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<BaseResponse<List<int>>> Handle(GetCountByStatusRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<int>>();

            var counts = await _unitOfWork.DriverRepository.GetCountByStatus();

            response.Success = true;
            response.Message = "Fetch Succesful";
            response.Value = counts;
            return response;
        }
    }
}