using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class GetDriversListRequestHandler : IRequestHandler<GetDriversListRequest, BaseResponse<List<DriverDetailDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDriversListRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public async Task<BaseResponse<List<DriverDetailDto>>> Handle(GetDriversListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<DriverDetailDto>>();
            var drivers = await _unitOfWork.DriverRepository.GetDriversWithDetails(request.PageNumber, request.PageSize);


            response.Success = true;
            response.Message = "Fetch Succesful";
            response.Value = _mapper.Map<List<DriverDetailDto>>(drivers);


            return response;


        }
    }
}
