using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    UserId = "e2231227-14b9-4f5c-ac0a-580b0324cee6",
                },
                new ()
                {
                    Id = 2,
                    Title = "Test Title 2",
                    Content = "Test Content 2",
                    UserId = "e2231227-14b9-4f5c-ac0a-580b0324cee6",
                },
            };

            var mockRepo = new Mock<IFeedbackRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(feedbacks);

            mockRepo.Setup(r => r.Add(It.IsAny<Feedback>())).ReturnsAsync((Feedback feedback) =>
            {
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
