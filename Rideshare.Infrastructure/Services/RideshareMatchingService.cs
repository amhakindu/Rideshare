using Hangfire;
using NetTopologySuite.Geometries;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Domain.Entities;


namespace Rideshare.Infrastructure.Services;

public class RideshareMatchingService : IRideshareMatchingService
{
    private readonly double _radius;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapboxService _mapboxService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public RideshareMatchingService(IUnitOfWork unitOfWork, IMapboxService mapboxService, IBackgroundJobClient backgroundJobClient, double radius)
    {
        _unitOfWork = unitOfWork;
        _mapboxService = mapboxService;
        _backgroundJobClient = backgroundJobClient;
        _radius = radius;
    }
    public static double HaversineDistance(Point origin, Point dest)
    {
        const double r = 6378100; // meters
                
        var sdlat = Math.Sin((ToRadians(dest.Y) - ToRadians(origin.Y)) / 2);
        var sdlon = Math.Sin((ToRadians(dest.X) - ToRadians(origin.X)) / 2);
        var q = sdlat * sdlat + Math.Cos(ToRadians(origin.Y)) * Math.Cos(ToRadians(dest.Y)) * sdlon * sdlon;
        var d = 2 * r * Math.Asin(Math.Sqrt(q));
        Console.WriteLine(d);
        return d;
    }
    public static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
    public async Task<bool> MatchWithRideoffer(RideRequest rideRequest)
    {
        var rideOffers = await _unitOfWork.RideOfferRepository.GetAll(pageSize: int.MaxValue); 
        RideOffer? matchedOffer = null;
        double optimumDetourDistance = double.MaxValue;
        foreach(RideOffer rideOffer in rideOffers){
            rideOffer.EstimatedDuration = TimeSpan.FromSeconds( await _mapboxService.GetEstimatedDuration(rideOffer.CurrentLocation.Coordinate, rideOffer.Destination.Coordinate));
            // Compute the shortest path between the current location of the rideoffer and the destination of the rideoffer
            List<Point> shortestPath = await _mapboxService.GetShortestPath(
                rideOffer.CurrentLocation.Coordinate,
                rideOffer.Destination.Coordinate
            );
            // Find nodes on this shortest path that are within radius _radius to the riderequest origin 
            // and sort it using the distance of each node in it to the riderequest origin
            var onPathNearRRorigin = shortestPath.Where(
                (point) => HaversineDistance(point, rideRequest.Origin.Coordinate) < _radius
            ).ToList();

            // Find nodes on this shortest path that are within radius _radius to the riderequest destination
            // and sort it using the distance of each node in it to the riderequest destination
            var onPathNearRRdestination = shortestPath.Where(
                point => HaversineDistance(point, rideRequest.Destination.Coordinate) < _radius
            ).ToList();

            foreach(var source in onPathNearRRorigin){
                foreach(var sink in onPathNearRRdestination){
                    //Check if they are traveling in the same direction
                    bool sameDirection = shortestPath.IndexOf(source) < shortestPath.IndexOf(sink);
                    if(sameDirection && rideOffer.AvailableSeats >= rideRequest.NumberOfSeats){
                        double avgDetourDistance = (HaversineDistance(source, rideRequest.Origin.Coordinate) + HaversineDistance(sink, rideRequest.Destination.Coordinate)) / 2;
                        // double avgDetourDistance = (originDistances[source] + destinationDistances[sink]) / 2;
                        if(matchedOffer == null || avgDetourDistance < optimumDetourDistance){
                            matchedOffer = rideOffer;
                            optimumDetourDistance = avgDetourDistance;
                        }
                    }
                }
            }
        }
        if(matchedOffer != null){
            matchedOffer.AvailableSeats -= 1;
            rideRequest.MatchedRide = matchedOffer;
            matchedOffer.Matches.Add(rideRequest);
            int operations = await _unitOfWork.RideOfferRepository.Update(matchedOffer);
            operations += await _unitOfWork.RideRequestRepository.Update(rideRequest);
            if(operations < 2)
                throw new InternalServerErrorException("Unable To Make Update Matched RideOffer and RideRequest");
        }
        return matchedOffer != null;
    }
}
