﻿using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Drivers.Validators;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateDriverCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;

        }

        public async Task<BaseResponse<int>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<int>();
            var validator = new CreateDriverDtoValidator();

            var validationResult = await validator.ValidateAsync(request.CreateDriverDto);


            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());

            if (await _userRepository.FindByIdAsync(request.CreateDriverDto.UserId) == null)
                throw new NotFoundException("User Not Found");

            var driver = _mapper.Map<Driver>(request.CreateDriverDto);

            if (await _unitOfWork.DriverRepository.Add(driver) == 0)
                throw new InternalServerErrorException("Database Error: Unable To Save");

            response.Success = true;
            response.Message = "Creation Succesful";
            response.Value = driver.Id;



            return response;


        }
    }
}
