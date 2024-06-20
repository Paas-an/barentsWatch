using BwConsumer.Service;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BwConsumer.Tests
{
    [TestFixture]
    public class OAuth2ServiceTest
    {
        [Test]
        public async Task MakeOAuth2Request_ShouldReturnToken()
        {
            // Arrange
            var oauthService = new OAuth2Service();
            var filePath = "info/token.json";

            // Act
            await oauthService.MakeOAuth2Request();
            var fileExists = File.Exists(filePath);

            // Assert
            Assert.IsTrue(fileExists);
        }

        [Test]
        public async Task MakeOAuth2Request_ShouldSaveBearerTokenToFile()
        {
            // Arrange
            var oauthService = new OAuth2Service();
            var filePath = "info/token.json";

            // Act
            await oauthService.MakeOAuth2Request();
            var fileContent = await File.ReadAllTextAsync(filePath);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(fileContent));
        }

        [Test]
        public async Task MakeOAuth2Request_ShouldReturnSuccessResponse()
        {
            // Arrange
            var oauthService = new OAuth2Service();
            var httpClient = new HttpClient();

            // Act
            await oauthService.MakeOAuth2Request();
            var response = await httpClient.GetAsync("https://id.barentswatch.no/connect/token");

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}