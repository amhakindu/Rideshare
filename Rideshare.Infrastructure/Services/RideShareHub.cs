using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Features.Userss;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Infrastructure.Services;
[Authorize]
public class RideShareHub : Hub<IRideShareHubClient>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapboxService _mapboxService;
    private readonly IUserAccessor _userAccessor;

    public RideShareHub(IUnitOfWork unitOfWork, IMapboxService mapboxService, IUserAccessor userAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapboxService = mapboxService;
        _userAccessor = userAccessor;
    }

    public async Task UpdateLocation(LocationDto location)
    {
        var id = _userAccessor.GetUserId();
        var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(id);
        var geolocation = _mapper.Map<GeographicalLocation>(location);
        var rideOffer = await _unitOfWork.RideOfferRepository.GetActiveRideOfferByDriverId(driver.Id);
        await _unitOfWork.RideOfferRepository.UpdateCurrentLocation(rideOffer, geolocation);
    }

    public async Task AddPassenger(int rideRequestId)
    {
        var id = _userAccessor.GetUserId();
        var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(id);
        var rideOffer = await _unitOfWork.RideOfferRepository.GetActiveRideOfferByDriverId(driver.Id);
        var rideRequest = await _unitOfWork.RideRequestRepository.Get(rideRequestId);
        rideRequest.AddStatus = true;
        rideOffer.AvailableSeats -= rideRequest.NumberOfSeats;
        await _unitOfWork.RideOfferRepository.Update(rideOffer);
        await _unitOfWork.RideRequestRepository.Update(rideRequest);
        var userId = rideRequest.UserId;
        var connections = await _unitOfWork.ConnectionRepository.GetByUserId(userId);

        foreach (var connection in connections)
        {
            Clients.Client(connection.Id).Accepted(rideOffer.Id);
        }
    }


    public async override Task OnConnectedAsync()
    {
        var connection = new Connection
        {
            Id = Context.ConnectionId,
            ApplicationUserId = _userAccessor.GetUserId()
        };
        await _unitOfWork.ConnectionRepository.Add(connection);
    }
    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        var connection = new Connection
        {
            Id = Context.ConnectionId,
            ApplicationUserId = _userAccessor.GetUserId(),
        };
        await _unitOfWork.ConnectionRepository.Delete(connection);
    }

}
