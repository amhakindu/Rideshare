using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Exceptions;

namespace Rideshare.Infrastructure.Services;


public class MapboxService: IMapboxService
{
    public const string BASE_URL = "https://api.mapbox.com/";
    public  HttpClient _httpClient;
    private readonly string _apiKey;

    public MapboxService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }
    
    public async Task<List<Point>> GetShortestPath(Point origin, Point destination)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                BASE_URL+$"directions/v5/mapbox/driving/{origin.X},{origin.Y};{destination.X},{destination.Y}?geometries=geojson&access_token={_apiKey}"
            );
            dynamic parsedJson = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            if(parsedJson.routes.Count == 0)
                return new List<Point>();
            // Check if the response was successful
            if (response.IsSuccessStatusCode){
                JArray coordinates = parsedJson.routes[0].geometry.coordinates;
                return coordinates.Select((token) => {
                    var coordinate = token.ToObject<List<double>>();
                    return new Point(coordinate[0], coordinate[1]){ SRID = 4326 };
                }).ToList();
            }
            else
                throw new InternalServerErrorException("Invalid Latitude or Longitude or both");
        }
        catch (Exception ex){
            throw new InternalServerErrorException("Error Accessing Mapbox Directions API: " + ex.Message);
        }
    }

    public async Task<double> GetDistance(Point origin, Point destination){
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                BASE_URL+$"directions/v5/mapbox/driving/{origin.X},{origin.Y};{destination.X},{destination.Y}?geometries=geojson&access_token={_apiKey}"
            );
            dynamic parsedJson = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            if(parsedJson.routes.Count == 0)
                return 0;
            // Check if the response was successful
            if (response.IsSuccessStatusCode){
                return parsedJson.routes[0].distance;
            }
            else{
                throw new InternalServerErrorException("Invalid Latitude or Longitude or both");
            }
        }
        catch (Exception ex){
            throw new InternalServerErrorException("Error Accessing Mapbox Directions API: " + ex.Message);
        }
    }

    public async Task<double> GetEstimatedDuration(Point origin, Point destination)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                BASE_URL+$"directions/v5/mapbox/driving/{origin.X},{origin.Y};{destination.X},{destination.Y}?geometries=geojson&access_token={_apiKey}"
            );
            dynamic parsedJson = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            if(parsedJson.routes.Count == 0)
                return 0;
            if (response.IsSuccessStatusCode){
                return parsedJson.routes[0].duration;
            }
            else{
                throw new InternalServerErrorException("Invalid Latitude or Longitude or both");
            }
        }
        catch (Exception ex){
            throw new InternalServerErrorException("Error Accessing Mapbox Directions API: " + ex.Message);
        }
    }

    public async Task<string> GetAddressFromCoordinates(Point coordinate)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                BASE_URL+$"geocoding/v5/mapbox.places/{coordinate.X},{coordinate.Y}.json?access_token={_apiKey}"
            );
            dynamic parsedJson = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            if(parsedJson.features.Count == 0)
                return "Unknown";
            // Check if the response was successful
            if (response.IsSuccessStatusCode)
                return parsedJson.features[0].place_name;
            else
                throw new InternalServerErrorException("Invalid Latitude or Longitude or both");
        }
        catch (Exception ex){
            throw new InternalServerErrorException("Error Accessing Mapbox Directions API: " + ex.Message);
        }
    }
}