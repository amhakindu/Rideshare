using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Persistence.Repositories;
public class ConnectionRepository : GenericRepository<Connection>, IConnectionRepository
{
    private readonly RideshareDbContext _dbContext;
    public ConnectionRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Connection>> GetByUserId(string applicationUserId)
    {
        var connections = await _dbContext.Connections
            .Where(connection => connection.ApplicationUserId == applicationUserId)
            .ToListAsync();
        return connections;
    }
}
