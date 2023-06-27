using Microsoft.AspNetCore.SignalR;
using Rideshare.Application.Common.Dtos.RideOffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Services;
public interface IRideShareHubClient
{
    Task MatchFound();
    Task Accepted(int rideOfferId);
}
