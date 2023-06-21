using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Rates.Handlers;
public class GetRateListQueryHandler : IRequestHandler<GetRateListQuery, BaseResponse<List<RateDto>>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetRateListQueryHandler(IMapper mapper, IUnitOfWork work)
	{
		_mapper = mapper;
		_unitOfWork = work;
	}

	public async Task<BaseResponse<List<RateDto>>> Handle(GetRateListQuery request, CancellationToken cancellationToken)
	{
		IReadOnlyList<RateEntity> rates = await _unitOfWork.RateRepository.GetAll(request.PageNumber, request.PageSize);
		
		
		// var rateDtos = rates.Select(rate => _mapper.Map<RateDto>(rate)).ToList();
		var rateDtos = (List<RateDto>)_mapper.Map<IReadOnlyList<RateEntity>, IReadOnlyList<RateDto>>(rates);
		return new BaseResponse<List<RateDto>>()
		{
			Success = true,
			Value = rateDtos,
			Message = "Rates Fetched Successfully!"
		};
	}
}
