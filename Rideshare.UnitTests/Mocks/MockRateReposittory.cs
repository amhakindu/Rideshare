using Rideshare.Application.Contracts.Persistence;
using Moq;
using Rideshare.Domain.Entities;

namespace Rideshare.UnitTests.Mocks
{
	public class MockRateRepository
	{
		public static Mock<IRateRepository> GetRateRepository()
		{
			var rates = new List<RateEntity>
			{
			   new ()
				{
					Id = 1,
					Rate = 5.4,
					UserId = "1",
					DriverId = 2,
					Description = "Description 1",
				},

				 new ()
				{
					Id = 2,
					Rate = 2.4,
					UserId = "2",
					DriverId = 2,
					Description = "Description 2",
				},

				
				new ()
				{
					Id = 3,
					Rate = 4.4,
					UserId = "3",
					DriverId = 4,
					Description = "Description 3",
				},

			};

			var mockRepo = new Mock<IRateRepository>();

			mockRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(rates);
			
			mockRepo.Setup(r => r.Add(It.IsAny<RateEntity>())).ReturnsAsync((RateEntity rate) =>
			{
				rates.Add(rate);
				return 1;
			});

			mockRepo.Setup(r => r.Update(It.IsAny<RateEntity>())).ReturnsAsync((RateEntity rate) =>
			{
				var newRates = rates.Where((r) => r.Id != rate.Id);
				rates = newRates.ToList();
				rates.Add(rate);
				return 1;
			});

			mockRepo.Setup(r => r.Delete(It.IsAny<RateEntity>())).ReturnsAsync((RateEntity rate) =>
			{
			  if (rate != null)
			  {
				rate = rates.FirstOrDefault(d => d.Id == rate.Id);
				if (rate != null)
					rates.Remove(rate);
					return 1;
				}
				return 0;

			});

			mockRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((int id) =>
			{
				return rates.FirstOrDefault((r) => r.Id == id);
			});

		return mockRepo;
		}
	}
}
