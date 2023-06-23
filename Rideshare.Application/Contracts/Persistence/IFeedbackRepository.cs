using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Persistence
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<IReadOnlyList<Feedback>> GetAllByUserId(string UserId, int PageNumber, int PageSize);
    }
}
