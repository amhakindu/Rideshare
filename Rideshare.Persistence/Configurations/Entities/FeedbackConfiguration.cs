using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
	{
		public void Configure(EntityTypeBuilder<Feedback> builder)
		{
			builder.HasData(
				new Feedback
				{
					Id = 1,
					UserId = "2e4c2829-138d-4e7e-b023-123456789004",
					Title = "Great Experience",
					Content = "I had a great experience using the Rideshare system. The app was easy to use and the rides were reliable.",
					Rating = 4.5
				},
				new Feedback
				{
					Id = 2,
					UserId = "2e4c2829-138d-4e7e-b023-123456789003",
					Title = "Smooth Transactions",
					Content = "I appreciate the smooth transactions provided by the Rideshare system. The payment process was seamless.",
					Rating = 4.0
				},
				new Feedback
				{
					Id = 3,
					UserId = "2e4c2829-138d-4e7e-b023-123456789002",
					Title = "Efficient Service",
					Content = "The Rideshare system provides efficient service with quick response times. I highly recommend it.",
					Rating = 4.8
				},
				new Feedback
				{
					Id = 4,
					UserId = "2e4c2829-138d-4e7e-b023-123456789001",
					Title = "Improvement Needed",
					Content = "Although the Rideshare system is convenient, there is room for improvement in terms of driver availability.",
					Rating = 3.2
				},
			   
				new Feedback
				{
					Id = 5,
					UserId = "2e4c2829-138d-4e7e-b023-123456789005",
					Title = "Reliable Transportation",
					Content = "I rely on the Rideshare system for my daily commute. It has been a reliable mode of transportation for me.",
					Rating = 4.7
				}
			);
		}
	}
}
