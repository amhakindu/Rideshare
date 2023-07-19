
namespace Rideshare.Application.Common.Dtos.Drivers
{
    public class UpdateDriverDto
    {
        public int Id { get; set; }
        public List<double> Rate { get; set; }
        public double Experience { get; set; }
        public string Address { get; set; }

        public string LicenseNumber { get; set; }
        public string License { get; set; }
        public bool Verified { get; set; }
    }
}
