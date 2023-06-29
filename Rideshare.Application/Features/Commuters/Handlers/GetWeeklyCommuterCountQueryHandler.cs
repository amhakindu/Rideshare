using System.ComponentModel.DataAnnotations;
using System.Globalization;
using MediatR;
using Rideshare.Application.Common.Dtos.Commuter.Validators;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Commuters.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Commuters.Handlers
{
	public class GetWeeklyCommuterCountQueryHandler : IRequestHandler<GetWeeklyCommuterCountQuery, BaseResponse<WeeklyCommuterCountDto>>
	{
		private readonly IUserRepository _userRepository;

		public GetWeeklyCommuterCountQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		public async Task<BaseResponse<WeeklyCommuterCountDto>> Handle(GetWeeklyCommuterCountQuery request, CancellationToken cancellationToken)
		{
			var validator = new WeeklyCommuterCountRequestValidator();
			var validationResult = await validator.ValidateAsync((request.Year, request.Month));

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());
			}

			var commuters = await _userRepository.GetUsersByRoleAsync("Commuter", 1, (int)429496729);
			var weeklyCounts = new WeeklyCommuterCountDto
			{
				Year = request.Year,
				Month = request.Month,
				WeeklyCounts = new Dictionary<int, int>()
			};

			var firstDayOfMonth = new DateTime(request.Year, request.Month, 1);
			var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			var totalWeeks = (lastDayOfMonth.Day + (int)firstDayOfMonth.DayOfWeek - 1) / 7 + 1;

			var currentDay = firstDayOfMonth;
			var weekNumber = 1;

			while (currentDay.Month == request.Month && currentDay <= lastDayOfMonth && weekNumber <= totalWeeks)
			{
				var startDate = currentDay;
				var endDate = currentDay.AddDays(6) < lastDayOfMonth ? currentDay.AddDays(6) : lastDayOfMonth;
				Console.WriteLine($"\n\n\n\n\n\n {(startDate, endDate)}, \n\n\n\n\n\n\n\n");
				
				var count = commuters.PaginatedUsers.Count(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate.AddDays(1));
				weeklyCounts.WeeklyCounts.Add(weekNumber, count);
				currentDay = currentDay.AddDays(7);
				weekNumber++;
			}

			var response = new BaseResponse<WeeklyCommuterCountDto>
			{
				Success = true,
				Message = $"Weekly commuter count for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(request.Month)} {request.Year} fetched successfully",
				Value = weeklyCounts
			};

			return response;
		}
		
	}
}
