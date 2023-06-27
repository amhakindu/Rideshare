
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Commuter.Validators;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Commuters.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Commuters.Handlers;
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
		
		var validator = new MonthlyCommuterCountRequestValidator();
		var validationResult = await validator.ValidateAsync(request.Year);

		
		if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());
			}
			
		var commuters = await _userRepository.GetUsersByRoleAsync("Commuter", 1, (int)429496729);
		var monthlyCounts = new MonthlyCommuterCountDto
		{
			Year = request.Year,
			MonthlyCounts = new Dictionary<int, int>()
		};
		

		for (int month = 1; month <= 12; month++)
		{
			var startDate = new DateTime(request.Year, month, 1);
			var endDate = startDate.AddMonths(1).AddDays(-1);
			
			var count = commuters.PaginatedUsers.Count(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate);
			monthlyCounts.MonthlyCounts.Add(month, count);
		}

		var response = new BaseResponse<MonthlyCommuterCountDto>
		{
			Success = true,
			Message = $"Monthly commuter count for {request.Year} fetched Successfully",
			Value = monthlyCounts
		};

		return response;
	}
}
