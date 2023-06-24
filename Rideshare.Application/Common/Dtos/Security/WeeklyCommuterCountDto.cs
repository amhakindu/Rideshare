using System.Collections.Generic;

namespace Rideshare.Application.Common.Dtos.Security
{
    public class WeeklyCommuterCountDto
    {
        public Dictionary<string, int> WeeklyCounts { get; set; }
    }
}
