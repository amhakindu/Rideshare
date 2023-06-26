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
			
			var commuters = await _userRepository.GetUsersAsync();
			var weeklyCounts = new WeeklyCommuterCountDto
			{
				Year = request.Year,
				Month = request.Month,
				WeeklyCounts = new Dictionary<int, int>()
			};

			var firstDayOfMonth = new DateTime(request.Year, request.Month, 1);
			var currentDay = firstDayOfMonth;
			var weekNumber = 1;

			while (currentDay.Month == request.Month && weekNumber <= 4)
			{
				var startDate = currentDay;
				var endDate = startDate.AddDays(6);
				var count = commuters.Count(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate);
				weeklyCounts.WeeklyCounts.Add(weekNumber, count);
				currentDay = currentDay.AddDays(7);
				weekNumber++;
			}

			var response = new BaseResponse<WeeklyCommuterCountDto>
			{
				Success = true,
				Message = $"Weekly commuter count for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(request.Month)} {request.Year} fetched Successfully",
				Value = weeklyCounts
			};

			return response;
		}
	}
}
