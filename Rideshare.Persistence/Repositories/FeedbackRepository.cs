using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Persistence.Repositories
{
    public class FeedbackRepository: GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly RideshareDbContext _dbContext;
        public FeedbackRepository(RideshareDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<Feedback>> GetAllByUserId(string UserId, int pageNumber, int pageSize)
        {
            return await _dbContext.Set<Feedback>().AsNoTracking()
                .Where(p=>p.UserId == UserId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
