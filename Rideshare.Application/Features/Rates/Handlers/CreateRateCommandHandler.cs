using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Rates.Handlers;
public class CreateRateCommandHandler : IRequestHandler<CreateRateCommand, BaseResponse<Nullable<int>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRateCommandHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<int?>> Handle(CreateRateCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateRateDtoValidator();
        var validationResult = await validator.ValidateAsync(request.RateDto);

        if (validationResult.IsValid == false)
        {
            return new BaseResponse<Nullable<int>>
            {
                Success = false,
                Message = "Rate Creation Failed",
                Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList()
            };
        }

        RateEntity newRate = _mapper.Map<RateEntity>(request.RateDto);

        int operations = await _unitOfWork.RateRepository.Add(newRate);

        if (operations > 0)
        {
        return new BaseResponse<Nullable<int>>
        {
            Success = true,
            Message = "Rate Creation Success",
            Value = newRate.Id };
        }

        return new BaseResponse<Nullable<int>>
        {
            Success = true,
            Message = "Rate Creation Success",
            Value = newRate.Id
        };
    }
}