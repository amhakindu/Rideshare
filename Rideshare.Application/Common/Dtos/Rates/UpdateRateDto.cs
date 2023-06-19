namespace Rideshare.Application.Common.Dtos.Rates;

public class UpdateRateDto
{
	public string UserId { get; set; } //rater.
	// public int DriverId { get; set; }  //driver.
 	public int Id { get; set; }
	public double Rate { get; set; }
	public string Description { get; set; }
	
	// I assume DriverId and Rater to be uneditable.
}
