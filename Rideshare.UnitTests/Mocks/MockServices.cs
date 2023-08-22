using System.Net;
using Moq;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Services;

namespace Rideshare.UnitTests.Mocks;

public class MockServices
{
    public static Mock<IRideShareHubService> GetHubService(){
        var mockHubService = new Mock<IRideShareHubService>();
        mockHubService.Setup(hubService => hubService.MatchFound(It.IsAny<string>(), It.IsAny<RideRequestDto>()));
        return mockHubService;
    }
    
    public static Mock<IMapboxService> GetMapboxService(){
        var shortestPaths = new Dictionary<string, List<Point>>();
        double[][] coordinates = new double[][]
        {
            new double[] {38.747791, 8.994517},new double[] {38.747802, 8.994388},new double[] {38.748707, 8.994502},new double[] {38.750347, 8.994908},new double[] {38.751808, 8.995075},
            new double[] {38.754048, 8.99571}, new double[] {38.755544, 8.995404}, new double[] {38.757303, 8.995696}, new double[] {38.758748, 8.995661}, new double[] {38.760693, 8.995816},
            new double[] {38.762162, 8.995789}, new double[] {38.762492, 8.995844}, new double[] {38.762875, 8.996044}, new double[] {38.763609, 8.996671}, new double[] {38.765001, 8.997664},
            new double[] {38.765213, 8.998022}, new double[] {38.765715, 8.999364},new double[] {38.76602, 8.999745},new double[] {38.766601, 9.000155},new double[] {38.766772, 9.000091},
            new double[] {38.766988, 9.000214}, new double[] {38.76701, 9.000423}, new double[] {38.766847, 9.000568}
        };
        shortestPaths["38.7478,8.9945;38.7668,9.0004"] = Array.ConvertAll(coordinates, c => new Point(c[0], c[1]){SRID=4326}).ToList();

        coordinates = new double[][]
        {
            new double[] {38.764907, 8.987561}, new double[] {38.765105, 8.987787}, new double[] {38.765084, 8.988026},
            new double[] {38.765921, 8.990586}, new double[] {38.766475, 8.993216}, new double[] {38.766887, 8.998432},
            new double[] {38.76691, 9.000126}, new double[] {38.767024, 9.000361}, new double[] {38.766847, 9.000568}
        };
        shortestPaths["38.7648,8.9877;38.7668,9.0004"] = Array.ConvertAll(coordinates, c => new Point(c[0], c[1]){SRID=4326}).ToList();
        
        coordinates = new double[][]
        {
            new double[] { 38.744506, 9.010428 }, new double[] { 38.73894, 9.01106 }, new double[] { 38.738914, 9.010895 },
            new double[] { 38.74049, 9.010711 }, new double[] { 38.744495, 9.010307 }, new double[] { 38.748214, 9.01069 },
            new double[] { 38.749813, 9.011385 }, new double[] { 38.755693, 9.011977 }, new double[] { 38.758096, 9.011701 },
            new double[] { 38.762695, 9.010839 }, new double[] { 38.766693, 9.010521 }   
        };
        shortestPaths["38.7445,9.0105;38.7667,9.0106"] = Array.ConvertAll(coordinates, c => new Point(c[0], c[1]){SRID=4326}).ToList();

        var mockMapboxSerice = new Mock<IMapboxService>();

        mockMapboxSerice.Setup(service => service.GetShortestPath(It.IsAny<Point>(), It.IsAny<Point>()))
            .ReturnsAsync((Point origin, Point destination) => {
                return shortestPaths[$"{origin.X},{origin.Y};{destination.X},{destination.Y}"];
            });      
        mockMapboxSerice.Setup(service => service.GetAddressFromCoordinates(It.IsAny<Point>())).ReturnsAsync("Somewhere");
        mockMapboxSerice.Setup(service => service.GetDistance(It.IsAny<Point>(), It.IsAny<Point>())).ReturnsAsync(1000);
        mockMapboxSerice.Setup(service => service.GetEstimatedDuration(It.IsAny<Point>(), It.IsAny<Point>())).ReturnsAsync(1000);
        return mockMapboxSerice;
    }
}