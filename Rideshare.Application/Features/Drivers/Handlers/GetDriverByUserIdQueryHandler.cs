using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class GetDriverByUserIdQueryHandler : IRequestHandler<GetDriverByUserIdQuery, BaseResponse<DriverDetailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDriverByUserIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public async Task<BaseResponse<DriverDetailDto>> Handle(GetDriverByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DriverDetailDto>();

            var driver = await _unitOfWork.DriverRepository.GetDriverWithDetailsByUser(request.Id);

            if (driver == null)
                throw new NotFoundException("Driver Not Found");



            response.Success = true;
            response.Message = "Fetch Successful";
            response.Value = _mapper.Map<DriverDetailDto>(driver);




            return response;
        }
    }
}
