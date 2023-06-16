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

			if (rate == null)
				throw new NotFoundException("Resource Not Found");

			_mapper.Map(request.RateDto, rate);

			if (await _unitOfWork.RateRepository.Update(rate) == 0)
				throw new InternalServerErrorException("Database Error: Unable To Save");
			
			// var driver = await _unitOfWork.DriverRepository.Get(rate.DriverId);
			// driver.Rate[0] += request.RateDto.Rate - rate.Rate ;  //new_rate_value - old_rate_value
			// if  (await _unitOfWork.DriverRepository.Update(driver) == 0)
			//     throw new InternalServerErrorException("Database Error: Unable To Save");
				


			response.Success = true;
			response.Message = "Update Successful";
			response.Value = Unit.Value;



			return response;
			
			
		}

	}
}


