using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BwConsumer.Service
{
    public class OAuth2Service
    {
        public async Task MakeOAuth2Request()
        {
            var client = new HttpClient();

            // Set the URL
            var url = "https://id.barentswatch.no/connect/token";

            // Set the headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            // Set the body
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "projonas11@gmail.com:Fishingvessel-surveillance"),
                new KeyValuePair<string, string>("client_secret", "Snus12345678"), // use "client_secret" here
                new KeyValuePair<string, string>("scope", "ais")
            });

            // Make the POST request
            var response = await client.PostAsync(url, content);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                // Extract the bearer token from the response
                var tokenData = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                var bearerToken = tokenData.access_token;

                // Save the bearer token to a JSON file
                var filePath = "info/token.json"; // Change the file extension to ".json"
                await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(tokenData));
            }
            else
            {
                Console.WriteLine($"Request failed with status code {response.StatusCode}");

                // If the request failed, try to parse the response content as JSON and print the error_description
                var errorContent = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(errorContent);
                var root = doc.RootElement;
                if (root.TryGetProperty("error_description", out var errorDescription))
                {
                    Console.WriteLine($"Error description: {errorDescription.GetString()}");
                }
            }
        }
    }
}
