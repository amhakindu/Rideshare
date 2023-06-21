namespace Rideshare.Application.Common.Dtos.Rates;

public class CreateRateDto

{
	public int Id { get; set; }
	public string UserId { get; set; }
	public double Rate { get; set; }
	public int DriverId { get; set; }
	public string Description { get; set; }
}
