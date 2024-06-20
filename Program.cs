
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using BwConsumer.Service;
using Newtonsoft.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        var oauthService = new OAuth2Service();
        await oauthService.MakeOAuth2Request();
        var updater = new ShipPositionUpdater();
        await updater.UpdateAllShipPositions();

        var shipService = new ShipService("info/ships.json", "info/aalesund.json");
        shipService.PrintDistancesToHomePort();
    }
}
