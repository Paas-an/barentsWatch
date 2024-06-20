using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
public class ShipPositionUpdater
{
    private readonly HttpClient _client;
    private readonly string _token;
    private readonly List<Ship> _ships;

   public ShipPositionUpdater()
    {
        _client = new HttpClient();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(File.ReadAllText("info/token.json"));
        _token = tokenResponse.access_token;
        _ships = new List<Ship>(); // Initialize _ships with an empty list

        // Log the start of the constructor
        Console.WriteLine("ShipPositionUpdater constructor called.");

        try
        {
            _ships = JsonConvert.DeserializeObject<List<Ship>>(File.ReadAllText("info/ships.json"));
        }
        catch (Exception ex)
        {
            // Log the exception if deserialization fails
            Console.WriteLine($"Failed to deserialize ships.json: {ex.Message}");
        }

        // Log the end of the constructor
        Console.WriteLine("ShipPositionUpdater constructor completed.");
    }

private async Task UpdateShipPosition(int mmsi)
{
    var request = new HttpRequestMessage(HttpMethod.Post, "https://live.ais.barentswatch.no/v1/latest/combined");
    request.Headers.Add("Authorization", $"Bearer {_token}");

    var json = JsonConvert.SerializeObject(new { mmsi = new[] { mmsi } });
    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    var response = await _client.SendAsync(request);

    if (response.IsSuccessStatusCode)
    {
        var responseJson = await response.Content.ReadAsStringAsync();
        var newPositions = JsonConvert.DeserializeObject<List<Position>>(responseJson);

        var ship = _ships.Find(s => s.Mmsi == mmsi);
        if (ship != null && newPositions.Count > 0)
        {
            ship.Position = newPositions[0]; // Use the first position in the list
        }

        Console.WriteLine($"Successfully updated position for ship with MMSI {mmsi}");
    }
    else
    {
        Console.WriteLine($"Failed to update position for ship with MMSI {mmsi}");
    }
}

public async Task UpdateAllShipPositions()
{
    foreach (var ship in _ships)
    {
        Console.WriteLine($"Updating position for ship {ship.Name} (MMSI: {ship.Mmsi})");
        await UpdateShipPosition(ship.Mmsi);
        Console.WriteLine($"Updated position for ship {ship.Name} (MMSI: {ship.Mmsi}): {ship.Position.Latitude}, {ship.Position.Longitude}");
    }

    var json = JsonConvert.SerializeObject(_ships,Formatting.Indented);
    File.WriteAllText("info/ships.json", json);
    Console.WriteLine($"Saved updated ship positions to info/ships.json");
}
}