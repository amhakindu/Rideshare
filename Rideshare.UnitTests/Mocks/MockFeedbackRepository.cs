
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.UnitTests.Mocks
{
	public class MockFeedbackRepository
	{
		public static Mock<IFeedbackRepository> GetFeedbackRepository()
		{
			var feedbacks = new List<Feedback>
			{
				new ()
				{
					Id = 1,
					Title = "Test Title",
					Content = "Test Content",
					Rating = 2.3,
					UserId = "user1",
				},
				new ()
				{
					Id = 2,
					Rating = 2.5,
					Title = "Test Title 2",
					Content = "Test Content 2",
					UserId = "user2",
				},
			};

			var mockRepo = new Mock<IFeedbackRepository>();

			mockRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((int pageNumber, int pageSize) => {

				var response = new PaginatedResponse<Feedback>();
				var result = feedbacks.AsQueryable().Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();

				response.Count = feedbacks.Count();
				response.Value = result;
				return response;

			}
				);

			mockRepo.Setup(r => r.Add(It.IsAny<Feedback>())).ReturnsAsync((Feedback feedback) =>
			{
				feedback.Id = feedbacks.Count + 1;
				feedbacks.Add(feedback);
				
				return feedback.Id;
			});

			mockRepo.Setup(r => r.Update(It.IsAny<Feedback>())).ReturnsAsync((Feedback feedback) =>
			{
				var newFeadbacks = feedbacks.Where((r) => r.Id != feedback.Id);
				feedbacks = newFeadbacks.ToList();
				feedbacks.Add(feedback);
				return feedback.Id;
			});

			mockRepo.Setup(r => r.Delete(It.IsAny<Feedback>())).ReturnsAsync((Feedback feedback) =>
			{
				if (feedbacks.Exists(b => b.Id == feedback.Id))
				{
					feedbacks.Remove(feedbacks.Find(b => b.Id == feedback.Id)!);
					return feedback.Id;
				}
				return 0;
			});

			mockRepo.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync((int id) =>
			{
				var feedbackId = feedbacks.FirstOrDefault((r) => r.Id == id);
				return feedbackId != null;
			});

			mockRepo.Setup(r => r.Get(It.IsAny<int>()))!.ReturnsAsync((int id) =>
			{
				return feedbacks.FirstOrDefault((r) => r.Id == id);
			});

			return mockRepo;
		}
	}
}
