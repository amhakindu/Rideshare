using Microsoft.AspNetCore.SignalR;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Infrastructure.Services;
public class RideShareHubService : IRideShareHubService
{
    private readonly IHubContext<RideShareHub, IRideShareHubClient> _context;
    private readonly IUnitOfWork _unitOfWork;

    public RideShareHubService(IHubContext<RideShareHub, IRideShareHubClient> hubContext, IUnitOfWork unitOfWork)
    {
        _context = hubContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HasConnections(string applicationUserId)
    {
        var connections = await _unitOfWork.ConnectionRepository.GetByUserId(applicationUserId);
        return connections.Any();
    }

    public async Task MatchFound(string applicationUserId, RideRequestDto rideRequestDto)
    {
        var connections = await _unitOfWork.ConnectionRepository.GetByUserId(applicationUserId);
        foreach (var connection in connections)
        {
            await _context.Clients.Client(connection.Id).MatchFound(rideRequestDto);
        }
    }
}
