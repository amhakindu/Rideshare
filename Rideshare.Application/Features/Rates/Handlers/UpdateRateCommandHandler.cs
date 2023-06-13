using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Handlers;
public class UpdateRateCommandHandler : IRequestHandler<UpdateRateCommand, BaseResponse<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRateCommandHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<Unit>> Handle(UpdateRateCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateRateDtoValidator();
        var validationResult = await validator.ValidateAsync(request.RateDto);

        var rate = await _unitOfWork.RateRepository.Get(request.RateDto.Id);

        if (validationResult.IsValid == false || rate == null)
        {
            var response = new BaseResponse<Unit>
            {
                Success = false,
                Message = "Rate Update Failed"
            };
            if (rate == null)
            {
                var error = $"Rate with id={request.RateDto.Id} does not found";
                response.Errors.Add(error);
            }
            else
            {
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            }
            return response;
        }

        rate.Rate = request.RateDto.Rate;
        rate.Id = request.RateDto.Id;
        rate.Description = request.RateDto.Description ?? rate.Description;

        int operations = await _unitOfWork.RateRepository.Update(rate);

        if (operations > 0)
        {
            return new BaseResponse<Unit>
            {
                Success = true,
                Message = "Rate Updated Successfully",
            };
        }

        return new BaseResponse<Unit>
        {
            Success = true,
            Message = "Rate Updated Successfully",
        };
    }
}