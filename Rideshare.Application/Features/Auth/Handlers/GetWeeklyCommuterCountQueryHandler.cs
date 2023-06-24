using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Queries;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Handlers
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
            var commuters = await _userRepository.GetUsersByRoleAsync("Commuter");
            var weeklyCounts = new WeeklyCommuterCountDto
            {
                WeeklyCounts = new Dictionary<string, int>()
            };

            var firstDayOfMonth = new DateTime(request.Year, request.Month, 1);
            var currentDay = firstDayOfMonth;
            for (int weekNumber = 1; weekNumber <= 4; weekNumber++)
            {
                var startDate = GetWeekStartDate(request.Year, request.Month, weekNumber);
                var endDate = startDate.AddDays(6);
                var count = commuters.Count(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate);
                weeklyCounts.WeeklyCounts.Add($"Week {weekNumber}", count);
                currentDay = currentDay.AddDays(7);
            }

            var response = new BaseResponse<WeeklyCommuterCountDto>
            {
                Success = true,
                Message = "Fetched In Successfully",
                Value = weeklyCounts
            };

            return response;
        }

        private DateTime GetWeekStartDate(int year, int month, int weekNumber)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var firstDayOfWeek = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
            var startDate = firstDayOfWeek.AddDays((weekNumber - 1) * 7);
            return startDate;
        }
    }
}
