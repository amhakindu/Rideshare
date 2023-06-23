using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Features.Auth.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
public class GetCommuterCountQueryHandler : IRequestHandler<GetCommuterCountQuery, BaseResponse<List<CommuterCountDto>>>
{
	private readonly IUserRepository _userRepository;

	public GetCommuterCountQueryHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Dictionary<string, int>> Handle(GetCommuterCountQuery request, CancellationToken cancellationToken)
	{
		var users = await _userRepository.GetUsersAsync();

		var counts = new Dictionary<string, int>();

		if (request.Year.HasValue)
		{
			if (request.Month.HasValue && request.Week.HasValue)
			{
				// Fetch commuter count for a specific week in the specified year and month
				var startDate = GetWeekStartDate(request.Year.Value, request.Month.Value, request.Week.Value);
				var endDate = startDate.AddDays(6);
				var count = users.Count(u => u.Roles.Contains("Commuter") && u.CreatedAt >= startDate && u.CreatedAt <= endDate);
				counts.Add($"week{request.Week.Value}", count);
			}
			else if (request.Month.HasValue)
			{
				// Fetch commuter count for each week in the specified year and month
				for (int i = 1; i <= 4; i++)
				{
					var startDate = GetWeekStartDate(request.Year.Value, request.Month.Value, i);
					var endDate = startDate.AddDays(6);
					var count = users.Count(u => u.Roles.Contains("Commuter") && u.CreatedAt >= startDate && u.CreatedAt <= endDate);
					counts.Add($"week{i}", count);
				}
			}
			else
			{
				// Fetch commuter count for each month in the specified year
				for (int i = 1; i <= 12; i++)
				{
					var startDate = new DateTime(request.Year.Value, i, 1);
					var endDate = startDate.AddMonths(1).AddDays(-1);
					var count = users.Count(u => u.Roles.Contains("Commuter") && u.CreatedAt >= startDate && u.CreatedAt <= endDate);
					counts.Add($"month{i}", count);
				}
			}
		}
		else
		{
			// Fetch commuter count from 2022 till current time
			var startDate = new DateTime(2022, 1, 1);
			var endDate = DateTime.Now;
			var count = users.Count(u => u.Roles.Contains("Commuter") && u.CreatedAt >= startDate && u.CreatedAt <= endDate);
			counts.Add("overall", count);
		}

		return counts;
	}
}
