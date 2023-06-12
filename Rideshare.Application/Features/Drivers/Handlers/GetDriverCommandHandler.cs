using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
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
    public class GetDriverCommandHandler : IRequestHandler<GetDriverRequest, BaseResponse<DriverDetailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDriverCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public async Task<BaseResponse<DriverDetailDto>> Handle(GetDriverRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DriverDetailDto>();

            var driver = await _unitOfWork.DriverRepository.Get(request.Id);


            
                response.Success = true;
                response.Message = "Fetch Successful";
                response.Value = _mapper.Map<DriverDetailDto>(driver);

            
            

            return response;
        }
    }
}
