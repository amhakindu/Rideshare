using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Rideshare.Domain.Common;

public class GeographicalLocation: BaseEntity
{
    public Point Coordinate { get; set; }
    
    public string Address { get; set; }

    [NotMapped]
    public object UserData { get; set; }
}
