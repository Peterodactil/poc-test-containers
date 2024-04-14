using API;
using System.Net;
using System.Net.Http.Json;
using Testes.Fixture;

namespace Testes.Tests
{
    [Collection("API")]
    public class GetByIdTest(CustomWebApplicationFactory factory)
    {
        private readonly HttpClient _client = factory.HttpClient;

        [Fact]
        public async Task DeveObterPrimeiroRegistro()
        {
            var response = await _client.GetAsync($"WeatherForecast/{1}");
            var content = await response.Content.ReadFromJsonAsync<WeatherForecast>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, content?.Id ?? 0);
        }
    }
}