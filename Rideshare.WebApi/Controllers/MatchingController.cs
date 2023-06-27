using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Common.Dtos.Vehicles;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Features.Userss;
using Rideshare.Application.Features.Vehicles.Commands;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Responses;
using System.Net;
using Twilio.TwiML.Voice;
using Twilio.Types;
using Task = System.Threading.Tasks.Task;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchingController : BaseApiController
{
    private readonly IRideShareHubService _rideShareHubService;

    public MatchingController(IMediator mediator, IRideShareHubService rideShareHubService, IUserAccessor userAccessor) : base(mediator, userAccessor)
    {
        _rideShareHubService = rideShareHubService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        // TODO: replace with real handler
        var start = new LocationDto { Latitude = 1, Longitude = 2 };
        var destination = new LocationDto { Latitude = 3, Longitude = 2 };
        var passenger = new Passenger
        {
            name = "test passenger",
            currentLocation = start,
            destination = destination,
            seatsAllocated = 1   
        };
        var testReturn = new TestReturn
        {
            driverName = "test",
            driverImageURL = "test",
            driverRatingAverageOutOf5 = 1,
            driverReviews = 1,
            carModel = "v8",
            availableSeats = 1,
            carImageURL = "test",
            carPlateNumber = "aa5876",
            driverPhoneNumber = "098765456789",
            carLocation = new LocationDto { Latitude = 1, Longitude = 1 },
            passengersList = new List<Passenger> { passenger }
        };
        return Ok(testReturn);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post() // TODO: Accept real body params
    {
        HttpClient client = new HttpClient();
        var userId = _userAccessor.GetUserId();
        client.BaseAddress = new Uri("https://localhost:7169");
        client.PostAsync($"api/Matching/{userId}", null);
        return getResponse(HttpStatusCode.Created, new BaseResponse<Unit> { Success=true });
    }


    // TODO: do authorization to accept only calls from server
    [HttpPost("{Id}")]
    public async Task<IActionResult> IntenalPost(string Id)
    {
        Thread.Sleep(5000); // TODO: handler with matching algorithm call goes here
        await _rideShareHubService.MatchFound(Id);
        return NoContent();
    }
}


class TestReturn
{
    public string driverName { get; set; }
    public string driverImageURL { get; set; }
    public double driverRatingAverageOutOf5 { get; set; }
    public int driverReviews { get; set; }
    public string carModel { get; set; }
    public int availableSeats { get; set; }
    public string carImageURL { get; set; }
    public string carPlateNumber { get; set; }
    public string driverPhoneNumber { get; set; }
    public LocationDto carLocation { get; set; }
    public List<Passenger> passengersList { get; set; }
}

class Passenger
{
    public string name { get; set; }
    public LocationDto currentLocation { get; set; }
    public LocationDto destination { get; set; }
    public int seatsAllocated { get; set; }
}