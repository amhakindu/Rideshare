using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace Rideshare.Application.Contracts.Infrastructure;


public interface IMapboxService
{
    public Task<List<Point>> GetShortestPath(Point origin, Point destination);
    public Task<double> GetDistance(Point origin, Point destination);
    public Task<double> GetEstimatedDuration(Point origin, Point destination);
    public Task<string> GetAddressFromCoordinates(Point coordinate);
}