using System;
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class GetCountByStatusRequestHandler : IRequestHandler<GetCountByStatusRequest, BaseResponse<StatusDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCountByStatusRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<BaseResponse<StatusDto>> Handle(GetCountByStatusRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<StatusDto>();

            var counts = await _unitOfWork.DriverRepository.GetCountByStatus();

            response.Success = true;
            response.Message = "Fetch Succesful";
            response.Value = new StatusDto() { Active= counts[0], Idle= counts[1]};
            return response;
        }
    }
}