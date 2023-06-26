using AutoMapper;
using MediatR;
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
    public class ApproveDriverCommandHandler : IRequestHandler<ApproveDriverCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApproveDriverCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<BaseResponse<Unit>> Handle(ApproveDriverCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Unit>();
            var validator = new ApproveDriverDtoValidator();

            var validationResult = await validator.ValidateAsync(request.ApproveDriverDto);


            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());

            var driver = await _unitOfWork.DriverRepository.Get(request.ApproveDriverDto.Id);

            if (driver == null)
                throw new NotFoundException("Driver Not Found");

            _mapper.Map(request.ApproveDriverDto, driver);

            if (await _unitOfWork.DriverRepository.Update(driver) == 0)
                throw new InternalServerErrorException("Database Error: Unable To Save");


            response.Success = true;
            response.Message = "Update Successful";
            response.Value = Unit.Value;


            return response;
        }
    }
}
