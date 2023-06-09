using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Repositories;

public class TestEntityRepository : GenericRepository<TestEntity>, ITestEntityRepository
{
    public TestEntityRepository(RideshareDbContext dbContext) : base(dbContext)
    {
    }
}
