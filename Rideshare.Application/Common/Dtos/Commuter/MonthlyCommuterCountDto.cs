namespace Rideshare.Application.Common.Dtos.Security;
public class MonthlyCommuterCountDto
{
    public int Year { get; set; }
    public Dictionary<int, int> MonthlyCounts { get; set; }
}