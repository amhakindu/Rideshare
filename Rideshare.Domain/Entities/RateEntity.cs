using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities
{
	public class RateEntity : BaseEntity
	{
		public double Rate { get; set; }
		public int RaterId { get; set; }
		public int DriverId { get; set; }
		public string Description { get; set; }
		public Driver Driver { get; set; }

	}
		
}
