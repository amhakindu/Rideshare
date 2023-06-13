using Moq;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.UnitTests.Mocks;

public class MockTestEntityRepository
{
    public static Mock<ITestEntityRepository> GetTestEntityRepository(){
        return new Mock<ITestEntityRepository>();
    }
}
