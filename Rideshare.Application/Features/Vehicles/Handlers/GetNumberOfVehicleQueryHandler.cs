using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var res = await _unitOfWork.VehicleRepository.GetNoVehicle(request.option, request.Year, request.Month);
            return new BaseResponse<Dictionary<int, int>> {
                Value = res,
                Message=$"Number of vehicle registered fetched succesfully"
            };
        }
    }
}
