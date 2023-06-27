
using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Commuters.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Commuters.Handlers;
public class GetYearlyCommuterCountQueryHandler : IRequestHandler<GetYearlyCommuterCountQuery, BaseResponse<YearlyCommuterCountDto>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetYearlyCommuterCountQueryHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<BaseResponse<YearlyCommuterCountDto>> Handle(GetYearlyCommuterCountQuery request, CancellationToken cancellationToken)
	{
		
		
		var commuters = await _userRepository.GetUsersByRoleAsync("Commuter", 1, (int)429496729);
		var yearlyCounts = new YearlyCommuterCountDto
		{
			YearlyCounts = new Dictionary<string, int>()
		};

		int currentYear = DateTime.UtcNow.Year;
		for (int year = 2022; year <= currentYear; year++)
		{
			var startDate = new DateTime(year, 1, 1);
			var endDate = startDate.AddYears(1).AddDays(-1);
			var count = commuters.PaginatedUsers.Count(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate);
			yearlyCounts.YearlyCounts.Add(year.ToString(), count);
		}

		var response = new BaseResponse<YearlyCommuterCountDto>();
		response.Success = true;
		response.Message = "Fetched In Successfully";
		response.Value = yearlyCounts;
		return response;
	}
}
