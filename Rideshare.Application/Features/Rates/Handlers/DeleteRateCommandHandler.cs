using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Rates.Handlers
{
	public class DeleteRateCommandHandler : IRequestHandler<DeleteRateCommand, BaseResponse<Unit>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public DeleteRateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<BaseResponse<Unit>> Handle(DeleteRateCommand request, CancellationToken cancellationToken)
		{
			// var validator = new DeleteRateValidator(_unitOfWork);
			// var validatorResult = await validator.ValidateAsync(request.RateId);

			// if (!validatorResult.IsValid)
			// {
			// 	throw new ValidationException(validatorResult.Errors.Select(e => e.ErrorMessage).ToList().First());
			// }

			// var rate = await _unitOfWork.RateRepository.Get(request.RateId);
			// if (rate == null)
			// {
			// 	throw new NotFoundException($"Rate with ID {request.RateId} does not exist");
			// }
			// var ops =  await _unitOfWork.RateRepository.Delete(rate);
			// if (ops == 0)
			// {
			// 	throw new InternalServerErrorException("Unable to Save to Database");
			// }
			// return new BaseResponse<int>
			// {
			// 	Success = true,
			// 	Message = "Rate delete Successful",
			// 	Value = request.RateId,
			
			
			
			var response = new BaseResponse<Unit>();

            var rate = await _unitOfWork.RateRepository.Get(request.RateId);

            if (rate == null)
                throw new NotFoundException("Resource Not Found");


            if (await _unitOfWork.RateRepository.Delete(rate) == 0)
                throw new InternalServerErrorException("Database Error: Unable To Save");

            response.Success = true;
            response.Message = "Deletion Succeeded";
            response.Value = Unit.Value;


            return response;

		}
	}
}

