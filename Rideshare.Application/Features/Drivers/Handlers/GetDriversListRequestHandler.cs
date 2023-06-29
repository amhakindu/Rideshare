using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Pagination;
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
    public class GetDriversListRequestHandler : IRequestHandler<GetDriversListRequest, BaseResponse<PaginatedResponseDto<DriverDetailDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDriversListRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public async Task<BaseResponse<PaginatedResponseDto<DriverDetailDto>>> Handle(GetDriversListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResponseDto<DriverDetailDto>>();
            var result = await _unitOfWork.DriverRepository.GetDriversWithDetails(request.PageNumber, request.PageSize);


            response.Success = true;
            response.Message = "Fetch Succesful";
            response.Value = new PaginatedResponseDto<DriverDetailDto>();
            response.Value.Count = result.Count;
            response.Value.PageNumber = request.PageNumber;
            response.Value.PageSize = request.PageSize;
            response.Value.Paginated = _mapper.Map<List<DriverDetailDto>>(result.Paginated);


            return response;


        }
    }
}
