using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Rates.Handlers;
public class GetRateListQueryHandler : IRequestHandler<GetRateListQuery, BaseResponse<IList<RateDto>>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetRateListQueryHandler(IMapper mapper, IUnitOfWork work)
	{
		_mapper = mapper;
		_unitOfWork = work;
	}

	public async Task<BaseResponse<IList<RateDto>>> Handle(GetRateListQuery request, CancellationToken cancellationToken)
	{
		IReadOnlyList<RateEntity> rates = await _unitOfWork.RateRepository.GetAll();

		var rateDtos = rates.Select(rate => _mapper.Map<RateDto>(rate)).ToList();
		return new BaseResponse<IList<RateDto>>()
		{
			Success = true,
			Value = rateDtos
		};
	}
}
