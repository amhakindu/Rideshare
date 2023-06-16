namespace Rideshare.Application.Common.Dtos;

public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public override bool Equals(object obj)
    {
        
        if (obj == null || GetType() != obj.GetType())
            return false;
        LocationDto other = (LocationDto)obj;
        return other.Longitude == Longitude && other.Latitude == Latitude;
    }
}
