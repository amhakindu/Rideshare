using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteDriverCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }


        public async Task<BaseResponse<Unit>> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Unit>();

            var driver = await _unitOfWork.DriverRepository.Get(request.Id);

            if (driver == null || driver.UserId != request.UserId)
                throw new NotFoundException("Resource Not Found");


            if (await _unitOfWork.DriverRepository.Delete(driver) == 0)
                throw new InternalServerErrorException("Database Error: Unable To Save");

            response.Success = true;
            response.Message = "Deletion Succeeded";
            response.Value = Unit.Value;


            return response;

        }
    }
}
