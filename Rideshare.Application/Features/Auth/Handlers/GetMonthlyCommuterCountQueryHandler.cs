
using System.Globalization;
using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Handlers;
public class GetMonthlyCommuterCountQueryHandler : IRequestHandler<GetMonthlyCommuterCountQuery, BaseResponse<MonthlyCommuterCountDto>>
{
	private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetMonthlyCommuterCountQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

	public async Task<BaseResponse<MonthlyCommuterCountDto>> Handle(GetMonthlyCommuterCountQuery request, CancellationToken cancellationToken)
	{
		var commuters = await _userRepository.GetUsersByRoleAsync("Commuter");
		var monthlyCounts = new MonthlyCommuterCountDto
		{
			MonthlyCounts = new Dictionary<string, int>()
		};

		for (int month = 1; month <= 12; month++)
		{
			var startDate = new DateTime(request.Year, month, 1);
			var endDate = startDate.AddMonths(1).AddDays(-1);
			var count = commuters.Count(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate);
			monthlyCounts.MonthlyCounts.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), count);
		}

		var response = new BaseResponse<MonthlyCommuterCountDto>
		{
			Success = true,
			Message = "Fetched In Successfully",
			Value = monthlyCounts
		};

		return response;
	}
}
