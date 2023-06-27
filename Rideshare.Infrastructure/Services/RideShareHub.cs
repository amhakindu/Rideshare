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

    public async Task SendLocation(LocationDto locationDto)
    {
        var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(_userAccessor.GetUserId());
        var driverOffers = (IReadOnlyList<RideOffer>)(await _unitOfWork.RideOfferRepository.GetRideOffersOfDriver(driver.Id, PageSize: int.MaxValue))["rideoffers"];
        foreach (RideOffer offer in driverOffers)
        {
            var newCoordinate = new Point(locationDto.Latitude, locationDto.Longitude);
            var geocode = await _mapboxService.GetAddressFromCoordinates(newCoordinate);
            var newLocation = new GeographicalLocation(){
                Coordinate = newCoordinate,
                Address = geocode
            };
            await _unitOfWork.RideOfferRepository.UpdateCurrentLocation(offer, newLocation);
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
