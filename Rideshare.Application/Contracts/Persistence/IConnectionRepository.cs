using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Persistence;
public interface IConnectionRepository
{
    Task<IReadOnlyList<Connection>> GetByUserId(string ApplicationUserId);
    Task<int> Add(Connection connection);
    Task<int> Delete(Connection connection);

}
