using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
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
		
	    var response = new BaseResponse<RateDto>();
        var rate = await _unitOfWork.RateRepository.Get(request.RateId);
        if (rate != null){
            response.Message = "Get Successful";
            response.Value = _mapper.Map<RateDto>(rate);

        }
        else{
            throw new NotFoundException($"Rate with {request.RateId} not found");
        }

        return response;
	}
}