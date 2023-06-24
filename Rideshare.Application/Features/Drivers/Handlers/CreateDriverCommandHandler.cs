using System.ComponentModel.Design;
using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Drivers.Validators;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
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
        private readonly IResourceManager _resourceManager;

        public CreateDriverCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IResourceManager resourceManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _resourceManager = resourceManager;

        }

        public async Task<BaseResponse<int>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<int>();
            var validator = new CreateDriverDtoValidator();

            var validationResult = await validator.ValidateAsync(request.CreateDriverDto);


            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());

            

            var driver = _mapper.Map<Driver>(request.CreateDriverDto);

            driver.UserId = request.UserId;
            driver.License= (await _resourceManager.UploadImage(request.CreateDriverDto.License)).AbsoluteUri;

            if (await _unitOfWork.DriverRepository.Add(driver) == 0)
                throw new InternalServerErrorException("Database Error: Unable To Save");

            response.Success = true;
            response.Message = "Creation Succesful";
            response.Value = driver.Id;



            return response;


        }
    }
}
