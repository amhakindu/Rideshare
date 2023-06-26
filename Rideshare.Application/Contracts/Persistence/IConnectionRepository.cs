using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Persistence;
public interface IConnectionRepository : IGenericRepository<Connection>
{
    Task<IReadOnlyList<Connection>> GetByUserId(string ApplicationUserId);
}
