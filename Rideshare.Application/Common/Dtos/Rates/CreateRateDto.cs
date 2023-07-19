namespace Rideshare.Application.Common.Dtos.Rates;

public class CreateRateDto

{
	public double Rate { get; set; }
	public int DriverId { get; set; }
	public string Description { get; set; }
}
