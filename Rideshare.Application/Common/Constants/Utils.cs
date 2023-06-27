using NetTopologySuite.Geometries;

namespace Rideshare.Application.Common.Constants;

public class Utils{
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
    // Helper method to convert degrees to radians
    public static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}