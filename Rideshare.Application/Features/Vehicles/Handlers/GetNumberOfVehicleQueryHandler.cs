using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Vehicles.Queries;

namespace Rideshare.Application.Features.Vehicles.Handlers
{
    public class GetNumberOfVehicleQueryHandler : IRequestHandler<GetNumberOfVehicleQuery, BaseResponse<Dictionary<int, int>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNumberOfVehicleQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<Dictionary<int, int>>> Handle(GetNumberOfVehicleQuery request, CancellationToken cancellationToken)
        {
            var res = await _unitOfWork.VehicleRepository.GetEntityStatistics(request.Year, request.Month);
            return new BaseResponse<Dictionary<int, int>> {
                Value = res,
                Message=$"Number of vehicle registered fetched succesfully"
            };
        }
    }
}
