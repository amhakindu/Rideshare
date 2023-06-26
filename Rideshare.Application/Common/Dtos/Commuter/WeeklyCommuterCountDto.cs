using System.Collections.Generic;

namespace Rideshare.Application.Common.Dtos.Security
{
	public class WeeklyCommuterCountDto
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public Dictionary<int, int> WeeklyCounts { get; set; }
	}
}
