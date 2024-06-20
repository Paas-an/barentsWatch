public class GeoCalculator
{
    private const double EarthRadiusInKilometers = 6371.0;

    public static double CalculateDistanceInKilometers(Position pos1, Position pos2)
{
    var R = 6371; // Radius of the earth in km
    var latDistance = ConvertToRadians(pos2.Latitude - pos1.Latitude);
    var lonDistance = ConvertToRadians(pos2.Longitude - pos1.Longitude);
    var a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
            + Math.Cos(ConvertToRadians(pos1.Latitude)) * Math.Cos(ConvertToRadians(pos2.Latitude))
            * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    var distance = R * c; // convert to kilometers

    return distance;
}

private static double ConvertToRadians(double degree)
{
    return (Math.PI / 180) * degree;
}
}