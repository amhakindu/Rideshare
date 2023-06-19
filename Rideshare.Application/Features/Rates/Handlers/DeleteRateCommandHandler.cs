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
			
			var response = new BaseResponse<Unit>();

			var rate = await _unitOfWork.RateRepository.Get(request.Id);

			if (rate == null)
				throw new NotFoundException("Resource Not Found");
			
			// Check if the UserId in the request matches the UserId of the rate in the database
			if (rate.UserId != request.UserId)
				throw new UnauthorizedAccessException("You are not authorized to delete this rate.");



			if (await _unitOfWork.RateRepository.Delete(rate) == 0)
				throw new InternalServerErrorException("Database Error: Unable To Save");
			
			var driver = await _unitOfWork.DriverRepository.Get(rate.DriverId);
			driver.Rate[0] -= rate.Rate;
			driver.Rate[1] -= 1;
			
			double total, count;
			total = driver.Rate[0];
			count = driver.Rate[1];
			double average = (total / count);
			driver.Rate[2] = average;
			
			if  (await _unitOfWork.DriverRepository.Update(driver) == 0)
				throw new InternalServerErrorException("Database Error: Unable To Save");

			response.Success = true;
			response.Message = "Deletion Succeeded";
			response.Value = Unit.Value;


			return response;

		}
	}
}

