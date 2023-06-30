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
				},
				
				// .
				new GeographicalLocation
				{
					Id = 6,
					Coordinate = new Point( 38.776260394810954, 9.0168305393388),
					Address = "Adwa st."
				},
				new GeographicalLocation
				{
					Id = 7,
					Coordinate = new Point( 38.776260394810954, 9.0168305393388),
					Address = "Kazanchis"
				},
				new GeographicalLocation
				{
					Id = 8,
					Coordinate = new Point(38.765837010093314, 9.02128024268311),
					Address = "Zewuditu St."
				},
				new GeographicalLocation
				{
					Id = 9,
					Coordinate = new Point(38.780020530892436, 9.010811122368025),
					Address = "Hotel Tirago"
				},
				new GeographicalLocation
				{
					Id = 10,
					Coordinate = new Point(  38.78420477681654, 9.021640510650847),
					Address = "Signal Condominum"
				}
				
			);
		}
	}
}
