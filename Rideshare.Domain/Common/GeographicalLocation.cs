using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Rideshare.Domain.Common;

public class GeographicalLocation: BaseEntity
{
    public Point Coordinate { get; set; }
    
    public string Address { get; set; }

    public override bool Equals(object obj)
    {
        
        if (obj == null || GetType() != obj.GetType())
            return false;
        GeographicalLocation other = (GeographicalLocation)obj;
        return other.Coordinate.X == Coordinate.X && other.Coordinate.Y == Coordinate.Y;
    }
}
