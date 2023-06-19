using Rideshare.Domain.Common;
using Rideshare.Domain.Models;

namespace Rideshare.Domain.Entities
{
	public class RateEntity : BaseEntity
	{
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public double Rate { get; set; }
		public int DriverId { get; set; }
		public string Description { get; set; }
		public Driver Driver { get; set; }

	}
		
}
