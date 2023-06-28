namespace Rideshare.Application.Common.Dtos.Statistics;

public class EntityCountChangeDto
{
    public string Name { get; set; }
    public int CurrentCount { get; set; }
    public double PercentageChange { get; set; }  
}
