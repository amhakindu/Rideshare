using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class GeographicalLocationConfiguration : IEntityTypeConfiguration<GeographicalLocation>
	{
				DateTime GetRandomDate(DateTime start, DateTime end, Random random)
		{
			var range = (end - start).Days;
			return start.AddDays(random.Next(range));
		}
		public void Configure(EntityTypeBuilder<GeographicalLocation> builder)
		{
			var random = new Random();
			var startDate = new DateTime(2023, 1, 1);
			builder.HasData(
				new GeographicalLocation
				{
					Id = 1,
					Coordinate = new Point(38.75936996738306, 9.035087371212217),
					Address = "Arat Kilo",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 2,
					Coordinate = new Point(38.76190197250373,9.035903240442103),
					Address = "Romina Caffe",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 3,
					Coordinate = new Point(38.76419794324873,9.035235711209607),
					Address = "Minilik",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 4,
					Coordinate = new Point(38.779318352747055,9.040918717359784),
					Address = "German Embassy",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 5,
					Coordinate = new Point(38.76096180087135, 9.033933907204755),
					Address = "Adwa st.",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				
				// .
				new GeographicalLocation
				{
					Id = 6,
					Coordinate = new Point( 38.776260394810954, 9.0168305393388),
					Address = "Adwa st.",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 7,
					Coordinate = new Point( 38.776260394810954, 9.0168305393388),
					Address = "Kazanchis",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 8,
					Coordinate = new Point(38.765837010093314, 9.02128024268311),
					Address = "Zewuditu St.",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 9,
					Coordinate = new Point(38.780020530892436, 9.010811122368025),
					Address = "Hotel Tirago",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				},
				new GeographicalLocation
				{
					Id = 10,
					Coordinate = new Point(  38.78420477681654, 9.021640510650847),
					Address = "Signal Condominum",
					DateCreated= GetRandomDate(startDate, DateTime.Now, random)
				}
				
			);
		}
	}
}
