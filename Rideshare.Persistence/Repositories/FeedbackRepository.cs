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
        public FeedbackRepository(RideshareDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
