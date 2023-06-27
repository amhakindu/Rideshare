using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Features.Userss;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Infrastructure.Services;
[Authorize]
public class RideShareHub : Hub<IRideShareHubClient>
{
    private readonly IUnitOfWork _unitOfWork;

    public RideShareHub(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task SendLocation(LocationDto locationDto)
    {
        // var userId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;
        // var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(userId);
        // var driverOffers = await _unitOfWork.RideOfferRepository.GetRideOffersOfDriver(driver.Id.ToString());
        // foreach (RideOffer offer in driverOffers)
        // {
        //     var newLoc = new Point(locationDto.Latitude, locationDto.Longitude);
        //     offer.CurrentLocation = newLoc;
        //     await _unitOfWork.RideOfferRepository.Update(offer);
        // }
        
    }

    public async override Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;
        var connection = new Connection
        {
            Id = Context.ConnectionId,
            ApplicationUserId = userId
        };
        await _unitOfWork.ConnectionRepository.Add(connection);
    }
    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;

        var connection = new Connection
        {
            Id = Context.ConnectionId,
            ApplicationUserId = userId,
        };
        await _unitOfWork.ConnectionRepository.Delete(connection);
    }

}
