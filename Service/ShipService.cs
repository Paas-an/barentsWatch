using Newtonsoft.Json;

public class ShipService
{
    private List<Ship> _ships;
    private Position _homePort;

    public ShipService(string shipsFile, string homePortFile)
    {
        _homePort = LoadHomePort(homePortFile);
        _ships = LoadShips(shipsFile);
    }

   private Position LoadHomePort(string filename)
{
    var json = File.ReadAllText(filename);
    var position = JsonConvert.DeserializeObject<Position>(json);

    Console.WriteLine($"Loaded home port position from {filename}: {position.Latitude}, {position.Longitude}");

    return position;
}

    private List<Ship> LoadShips(string filename)
    {
        var json = File.ReadAllText(filename);
        return JsonConvert.DeserializeObject<List<Ship>>(json);
    }

    public double GetDistanceToHomePort(int mmsi)
    {
        var ship = _ships.Find(s => s.Mmsi == mmsi);
        return GeoCalculator.CalculateDistanceInKilometers(_homePort, ship.Position);
    }
    public Dictionary<int, double> GetDistancesToHomePort()
{
    var distances = new Dictionary<int, double>();

    foreach (var ship in _ships)
    {
        var distance = GeoCalculator.CalculateDistanceInKilometers(_homePort, ship.Position);
        distances[ship.Mmsi] = distance;

        
    }

    return distances;
}

public void PrintDistancesToHomePort()
{
    var distances = GetDistancesToHomePort();

    foreach (var entry in distances)
    {
        var ship = _ships.Find(s => s.Mmsi == entry.Key);
        var distance = Math.Round(entry.Value, 2); // Round to 2 decimal places
        Console.WriteLine($"The distance from the ship {ship.Name} (MMSI: {entry.Key}) to the home port is {distance} kilometers./n");
    }
}
}