using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.RideRequests;
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
    private readonly IMapper _mapper;

    public RideShareHub(IUnitOfWork unitOfWork, IMapboxService mapboxService, IUserAccessor userAccessor, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapboxService = mapboxService;
        _userAccessor = userAccessor;
        _mapper = mapper;
    }

    public async Task UpdateLocation(LocationDto location)
    {
        var id = _userAccessor.GetUserId();
        var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(id);
        var geolocation = _mapper.Map<GeographicalLocation>(location);
        var rideOffer = await _unitOfWork.RideOfferRepository.GetActiveRideOfferOfDriver(driver.Id);
        await _unitOfWork.RideOfferRepository.UpdateCurrentLocation(rideOffer, geolocation);
    }

    public async Task AddPassenger(int rideRequestId)
    {
        var id = _userAccessor.GetUserId();
        Console.WriteLine($"Id found name: {id}");
        var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(id);
        Console.WriteLine($"Id found name: {driver.Id}");
        var rideOffer = await _unitOfWork.RideOfferRepository.GetRideOfferWithDetail(driver.Id);
     ;
      Console.WriteLine($"Id found name: {rideOffer.Id}");
        var rideRequest = await _unitOfWork.RideRequestRepository.Get(rideRequestId);
        await _unitOfWork.RideOfferRepository.AcceptRideRequest(rideRequestId);
        var userId = rideRequest.UserId;
        var connections = await _unitOfWork.ConnectionRepository.GetByUserId(userId);
        var commuterViewOfRideOfferDto = _mapper.Map<CommuterViewOfRideOfferDto>(rideOffer);
        foreach (var connection in connections)
        {
            await Clients.Client(connection.Id).Accepted(commuterViewOfRideOfferDto);
        }
    }

    public async Task Drop(int rideRequestId)
    {
        var rideRequest = await _unitOfWork.RideRequestRepository.Get(rideRequestId);
        var userId = rideRequest.UserId;
        var connections = await _unitOfWork.ConnectionRepository.GetByUserId(userId);
        foreach (var connection in connections)
        {
            await Clients.Client(connection.Id).OnDrop();
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
