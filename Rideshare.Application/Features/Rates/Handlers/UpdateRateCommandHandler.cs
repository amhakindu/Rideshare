using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Rates.Handlers
{
	public class UpdateRateCommandHandler: IRequestHandler<UpdateRateCommand, BaseResponse<Unit>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateRateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<BaseResponse<Unit>> Handle(UpdateRateCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<Unit>();
			var validator = new UpdateRateDtoValidator();

			var validationResult = await validator.ValidateAsync(request.RateDto);


			if (!validationResult.IsValid)
				throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());

			var rate = await _unitOfWork.RateRepository.Get(request.RateDto.Id);
			var old_rate = rate.Rate;

			if (rate == null)
				throw new NotFoundException("Resource Not Found");
			
			// Check if the UserId in the request matches the UserId of the rate in the database
            if (rate.UserId != request.RateDto.UserId)
                throw new UnauthorizedAccessException("You are not authorized to update this rate.");


			_mapper.Map(request.RateDto, rate);

			if (await _unitOfWork.RateRepository.Update(rate) == 0)
				throw new InternalServerErrorException("Database Error: Unable To Save");
			
			var driver = await _unitOfWork.DriverRepository.Get(rate.DriverId);
			driver.Rate[0] += request.RateDto.Rate - old_rate ;  //new_rate_value - old_rate_value
			
			double total, count;
			total = driver.Rate[0];
			count = driver.Rate[1];
            double average = Math.Round(total / count, 2);  
			driver.Rate[2] = average;
			
			if  (await _unitOfWork.DriverRepository.Update(driver) == 0)
				throw new InternalServerErrorException("Database Error: Unable To Save");
				


			response.Success = true;
			response.Message = "Update Successful";
			response.Value = Unit.Value;



			return response;
			
			
		}

	}
}


