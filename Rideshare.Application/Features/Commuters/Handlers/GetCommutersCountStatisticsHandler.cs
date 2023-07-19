using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Commuters.Queries;

namespace Rideshare.Application.Features.Commuters.Handlers;

public class GetCommutersCountStatisticsHandler : IRequestHandler<GetCommutersCountStatisticsQuery, BaseResponse<Dictionary<int, int>>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetCommutersCountStatisticsHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<BaseResponse<Dictionary<int, int>>> Handle(GetCommutersCountStatisticsQuery request, CancellationToken cancellationToken)
	{
        var history = await _userRepository.GetCommuterStatistics(request.Year, request.Month);
		return new BaseResponse<Dictionary<int, int>>
		{
			Success = true,
			Message = $"Monthly commuter count for {request.Year} fetched Successfully",
			Value = history
		};
	}
}
