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
	public class UpdateRateCommandHandler: IRequestHandler<UpdateRateCommand, BaseResponse<int>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateRateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<BaseResponse<int>> Handle(UpdateRateCommand request, CancellationToken cancellationToken)
		{
			var validator = new UpdateRateDtoValidator(_unitOfWork);
			var validatorResult = await validator.ValidateAsync(request.RateDto);

			if (!validatorResult.IsValid)
			{
				throw new ValidationException(validatorResult.Errors.Select(e => e.ErrorMessage).ToList().First());
			}
			
			var rate = _mapper.Map<RateEntity>(request.RateDto);
			var noOperations = await _unitOfWork.RateRepository.Update(rate);
			if (noOperations == 0)
			{

				throw new InternalServerErrorException("Unable to Save to Database");
			}
			return new BaseResponse<int> {
				Success = true,
				Message = "Rate Creation Successful",
				Value = request.RateDto.Id,
			};
		}

	}
}


