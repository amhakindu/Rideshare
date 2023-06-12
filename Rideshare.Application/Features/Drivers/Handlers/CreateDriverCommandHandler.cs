using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Drivers.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDriverCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }

        public async Task<BaseResponse<int>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<int>();
            var validator = new CreateDriverDtoValidator();

            var validationResult = await validator.ValidateAsync(request.CreateDriverDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();

            }
            else
            {
                var driver = _mapper.Map<Driver>(request.CreateDriverDto);

                if (await _unitOfWork.DriverRepository.Add(driver) > 0)
                {
                    response.Success = true;
                    response.Message = "Creation Succesful";
                    response.Value = driver.Id;
                }
                else
                {

                    response.Success = false;
                    response.Message = "Creation Failed";
                }

            }
            return response;


        }
    }
}
