using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Common.Dtos.Drivers.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Drivers.Handlers
{
    public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDriverCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<BaseResponse<Unit>> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Unit>();
            var validator = new UpdateDriverDtoValidator();

            var validationResult = await validator.ValidateAsync(request.UpdateDriverDto);


            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());

            var driver = await _unitOfWork.DriverRepository.Get(request.UpdateDriverDto.Id);

            if (driver == null)
                throw new NotFoundException("Resource Not Found");

            _mapper.Map(request.UpdateDriverDto, driver);

            if (await _unitOfWork.DriverRepository.Update(driver) == 0)
                throw new InternalServerErrorException("Database Error: Unable To Save");


            response.Success = true;
            response.Message = "Update Successful";
            response.Value = Unit.Value;



            return response;


        }
    }
}
