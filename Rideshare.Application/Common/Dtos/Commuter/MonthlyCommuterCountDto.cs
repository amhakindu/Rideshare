namespace Rideshare.Application.Common.Dtos.Security;
public class MonthlyCommuterCountDto
{
    public int Year { get; set; }
    public Dictionary<string, int> MonthlyCounts { get; set; }
}