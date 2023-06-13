using Rideshare.Application.Contracts.Persistence;
using Moq;
using Rideshare.Domain.Entities;

namespace Rideshare.UnitTests.Mocks
{
    public static class MockRateRepository
	{
		public static Mock<IRateRepository> GetRateRepository()
		{
			var rates = new List<RateEntity>
			{
			   new RateEntity
				{
					Id = 1,
					Rate = 5.4,
					RaterId = 3,
					DriverId = 2,
					Description = "Description 2",
				},

				 new RateEntity
				{
					Id = 2,
					Rate = 2.4,
					RaterId = 2,
					DriverId = 2,
					Description = "Description 3",
				},

				
				new RateEntity
				{
					Id = 3,
					Rate = 4.4,
					RaterId = 3,
					DriverId = 4,
					Description = "Description 1",
				},

			};

			var mockRepo = new Mock<IRateRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(rates);
            mockRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((int id) => rates.FirstOrDefault(d => d.Id == id));
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


            return mockRepo;

        }
    }
}