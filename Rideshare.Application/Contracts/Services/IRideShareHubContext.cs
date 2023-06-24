using Rideshare.Application.Common.Dtos.RideOffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Services;
public interface IRideShareHubService
{
    Task MatchFound(string applicationUserId);
    Task<bool> HasConnections(string applicationUserId);
}
