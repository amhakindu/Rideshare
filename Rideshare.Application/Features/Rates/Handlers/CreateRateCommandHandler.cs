using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Rates.Handlers;
public class CreateRateCommandHandler : IRequestHandler<CreateRateCommand, BaseResponse<int>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public CreateRateCommandHandler(IMapper mapper, IUnitOfWork work)
	{
		_mapper = mapper;
		_unitOfWork = work;
	}

	public async Task<BaseResponse<int>> Handle(CreateRateCommand request, CancellationToken cancellationToken)
	{
		var validator = new CreateRateDtoValidator();
		var validationResult = await validator.ValidateAsync(request.RateDto);

		if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());
			}
			
		
			var rate = _mapper.Map<RateEntity>(request.RateDto);
			var Operations = await _unitOfWork.RateRepository.Add(rate);

			if (Operations == 0)
			{
			   throw new InternalServerErrorException("Unable To Save To Database");
			}
			
			var driver = await _unitOfWork.DriverRepository.Get(request.RateDto.DriverId);
			driver.Rate[0] += rate.Rate;
			driver.Rate[1] += 1;
			
			double total, count;
			total = driver.Rate[0];
			count = driver.Rate[1];
			double average ;
			average = (double)total / count;  
			driver.Rate[2] = average;
			
			if  (await _unitOfWork.DriverRepository.Update(driver) == 0)
				throw new InternalServerErrorException("Database Error: Unable To Save");
				
			return new BaseResponse<int>
			{
				Success = true,
				Message = "Rate Creation Successful",
				Value = request.RateDto.Id,
			};
		}
	}
