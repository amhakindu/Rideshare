using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
	{
		DateTime GetRandomDate(DateTime start, DateTime end, Random random)
		{
			var range = (end - start).Days;
			return start.AddDays(random.Next(range));
		}
		public void Configure(EntityTypeBuilder<Feedback> builder)
		{
			var random = new Random();
			var startDate = new DateTime(2023, 1, 1);
			builder.HasData(
				new Feedback
				{
					Id = 1,
					UserId = "2e4c2829-138d-4e7e-b023-123456789004",
					Title = "Great Experience",
					Content = "I had a great experience using the Rideshare system. The app was easy to use and the rides were reliable.",
					Rating = 4.5,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 2,
					UserId = "2e4c2829-138d-4e7e-b023-123456789003",
					Title = "Smooth Transactions",
					Content = "I appreciate the smooth transactions provided by the Rideshare system. The payment process was seamless.",
					Rating = 4.0,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 3,
					UserId = "2e4c2829-138d-4e7e-b023-123456789002",
					Title = "Efficient Service",
					Content = "The Rideshare system provides efficient service with quick response times. I highly recommend it.",
					Rating = 4.8,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 4,
					UserId = "2e4c2829-138d-4e7e-b023-123456789001",
					Title = "Improvement Needed",
					Content = "Although the Rideshare system is convenient, there is room for improvement in terms of driver availability.",
					Rating = 3.2,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
			   
				new Feedback
				{
					Id = 5,
					UserId = "2e4c2829-138d-4e7e-b023-123456789005",
					Title = "Reliable Transportation",
					Content = "I rely on the Rideshare system for my daily commute. It has been a reliable mode of transportation for me.",
					Rating = 4.7,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 6,
					UserId = "345e6bcd-7890-4def-12ab-123456789005",
					Title = "Efficient and Reliable",
					Content = "I have been using the rideshare service for several months now, and I must say it is incredibly efficient and reliable. The drivers always arrive on time and take the shortest routes to my destination. I appreciate the convenience and peace of mind that comes with using this service. The app is user-friendly, making it easy to book rides and track the driver's location. Overall, I am highly satisfied with the level of service provided by this rideshare company.",
					Rating = 4.7,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 7,
					UserId = "678f9de0-1c23-4ab4-5678-123456789006",
					Title = "Prompt and Courteous",
					Content = "I have had the pleasure of riding with some of the most courteous and professional drivers through this rideshare service. They always arrive promptly and greet me with a smile. They maintain a clean and comfortable vehicle, ensuring a pleasant experience throughout the ride. The drivers also follow traffic rules and drive responsibly, making me feel safe and secure. I would highly recommend this rideshare service to anyone looking for prompt and courteous transportation.",
					Rating = 4.8,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 8,
					UserId = "90a1b234-c5d6-4e7f-8901-123456789007",
					Title = "Smooth Navigation",
					Content = "The navigation system of the app is remarkably smooth and accurate. It provides real-time updates on traffic conditions and suggests alternative routes to avoid congestion. I have never faced any issues with the app's navigation, and it has helped me reach my destination without any hassle. The turn-by-turn directions are clear and concise, ensuring I never miss a turn. I truly appreciate the effort put into developing such a reliable and user-friendly navigation system.",
					Rating = 4.5,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 9,
					UserId = "12b3c4d5-67e8-490f-1a2b-123456789008",
					Title = "Great Customer Support",
					Content = "I recently had a query regarding my ride, and I reached out to the customer support team of this rideshare service. I must say, they provided excellent assistance and resolved my issue promptly. The support team was friendly, patient, and attentive to my concerns. They went above and beyond to ensure my satisfaction and restored my faith in their commitment to customer service. I am extremely pleased with the level of support I received and would like to express my gratitude to the entire customer support team.",
					Rating = 4.3,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 10,
					UserId = "34e5f6a7-8901-4bcd-23ef-123456789009",
					Title = "Affordable and Convenient",
					Content = "This rideshare service has become my go-to option for transportation due to its affordability and convenience. The fares are competitively priced, making it an economical choice for daily commuting and occasional trips. The availability of rides is impressive, and I never had to wait too long for a driver to arrive. The convenience of booking rides through the app and the ease of payment add to the overall experience. I highly recommend this rideshare service to anyone looking for an affordable and convenient mode of transportation.",
					Rating = 4.6,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 11,
					UserId = "5678a9b0-c1d2-4e3f-4a5b-123456789010",
					Title = "Exceptional Service",
					Content = "I have been consistently impressed with the exceptional service provided by this rideshare company. The drivers are professional, punctual, and always go the extra mile to ensure a comfortable and enjoyable ride. The vehicles are clean and well-maintained, adding to the overall experience. The customer support team is responsive and addresses any concerns promptly. I highly recommend this rideshare service to anyone looking for a top-notch transportation option.",
					Rating = 4.9,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 12,
					UserId = "8901c2d3-4e56-47f8-9012-123456789011",
					Title = "Reliable and Safe",
					Content = "Reliability and safety are the top priorities for me when it comes to choosing a rideshare service, and this company exceeds my expectations on both fronts. The drivers are reliable, arriving on time and taking the most efficient routes to my destination. They prioritize safety by adhering to traffic rules and maintaining a smooth driving experience. I feel confident and secure during every ride. I appreciate the commitment to providing a reliable and safe transportation service.",
					Rating = 4.7,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 13,
					UserId = "1234e56f-7890-4c1d-23e4-123456789012",
					Title = "User-Friendly App",
					Content = "The user experience offered by the app of this rideshare service is remarkable. The interface is intuitive and easy to navigate, allowing me to book rides seamlessly. I can track the driver's location in real-time and receive notifications about the ride status. The app also provides clear fare estimates, ensuring transparency in pricing. It has truly simplified my transportation needs and enhanced my overall experience. I highly recommend using this user-friendly app for hassle-free rides.",
					Rating = 4.5,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 14,
					UserId = "4567f89a-0b12-43c4-5678-123456789013",
					Title = "Excellent Driver Behavior",
					Content = "I want to express my appreciation for the excellent behavior displayed by the drivers of this rideshare service. They are polite, courteous, and maintain a high level of professionalism. They create a pleasant and welcoming atmosphere during the ride, making me feel valued as a customer. Their friendly demeanor and engaging conversations add a personal touch to the service. I am grateful for the positive experiences I've had with the drivers and highly recommend this rideshare service.",
					Rating = 4.6,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new Feedback
				{
					Id = 15,
					UserId = "7890a1b2-34c5-4d6e-7890-123456789014",
					Title = "Convenient Payment Options",
					Content = "The variety of payment options offered by this rideshare service has made my life so much easier. I can choose to pay with cash or through the app using various digital payment methods. This flexibility ensures that I always have a convenient way to pay for my rides. The app securely stores my payment details, eliminating the need to carry cash. I appreciate the effort put into providing convenient and secure payment options, making the entire ride experience hassle-free.",
					Rating = 4.4,
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
					
				}
			);
		}
	}
}
