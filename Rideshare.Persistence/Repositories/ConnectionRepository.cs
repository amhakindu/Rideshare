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
public class ConnectionRepository : IConnectionRepository
{
    private readonly RideshareDbContext _dbContext;
    public ConnectionRepository(RideshareDbContext dbContext)
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

    public async Task<int> Add(Connection connection)
    {
        await _dbContext.AddAsync(connection);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> Delete(Connection connection)
    {
        _dbContext.Set<Connection>().Remove(connection);
        return await _dbContext.SaveChangesAsync();
    }

}
