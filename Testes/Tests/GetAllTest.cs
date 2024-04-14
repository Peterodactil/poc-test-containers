using API;
using System.Net;
using System.Net.Http.Json;
using Testes.Fixture;

namespace Testes.Tests
{
    [Collection("API")]
    public class GetAllTest(CustomWebApplicationFactory factory)
    {
        private readonly HttpClient _client = factory.HttpClient;

        [Fact]
        public async Task DeveObterTodosRegistros()
        {
            var response = await _client.GetAsync("WeatherForecast");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(4, content!.Count());
        }
    }
}