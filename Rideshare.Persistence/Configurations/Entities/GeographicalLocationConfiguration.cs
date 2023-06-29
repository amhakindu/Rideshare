using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Persistence.Configurations.Entities
{
    public class GeographicalLocationConfiguration : IEntityTypeConfiguration<GeographicalLocation>
    {
        public void Configure(EntityTypeBuilder<GeographicalLocation> builder)
        {
            builder.HasData(
                new GeographicalLocation
                {
                    Id = 1,
                    Coordinate = new Point(38.75936996738306, 9.035087371212217),
                    Address = "Arat Kilo"
                },
                new GeographicalLocation
                {
                    Id = 2,
                    Coordinate = new Point(38.76190197250373,9.035903240442103),
                    Address = "Romina Caffe"
                },
                new GeographicalLocation
                {
                    Id = 3,
                    Coordinate = new Point(38.76419794324873,9.035235711209607),
                    Address = "Minilik"
                },
                new GeographicalLocation
                {
                    Id = 4,
                    Coordinate = new Point(38.779318352747055,9.040918717359784),
                    Address = "German Embassy"
                },
                new GeographicalLocation
                {
                    Id = 5,
                    Coordinate = new Point(38.76096180087135, 9.033933907204755),
                    Address = "Adwa st."
                }
            );
        }
    }
}
