using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Responses;


namespace Rideshare.Application.Features.Rate.Handlers;
public class GetRateDetailQueryHandler : IRequestHandler<GetRateDetailQuery, BaseResponse<RateDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetRateDetailQueryHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<RateDto>> Handle(GetRateDetailQuery request, CancellationToken cancellationToken)
    {
        bool exists = await _unitOfWork.RateRepository.Exists(request.RateId);
        if (exists == false)
        {
            var error = $"Rate with id={request.RateId} was not found";
            return new BaseResponse<RateDto>
            {
                Success = false,
                Message = "Rate Fetch Failed",
                Errors = new List<string>() { error }
            };
        }
        var rate = await _unitOfWork.RateRepository.Get(request.RateId);
        return new BaseResponse<RateDto>
        {
            Success = true,
            Message = "Rate Fetch Success",
            Value = _mapper.Map<RateDto>(rate)
        };
    }
}