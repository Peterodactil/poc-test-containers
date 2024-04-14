using API;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Http.Json;
using System.Net;
using Testcontainers.MongoDb;
using Testes.Fixture;

namespace Testes.Tests
{
    [Collection("API")]
    public class MongoTest(CustomWebApplicationFactory factory)
    {
        // HttpClient deve ser chamado para garantir que os containers sejam iniciados.
        private readonly HttpClient _client = factory.HttpClient;
        private readonly MongoDbContainer _mongoDbContainer = MongoConfigurationProvider.Container;

        [Fact]
        public async Task DeveRetornarBancoDadosTest()
        {
            var mongoClient = new MongoClient(_mongoDbContainer.GetConnectionString());
            // o script executado gera dados em um banco de dados chamado 'test'
            var options = new ListDatabasesOptions() {
                Filter = Builders<BsonDocument>.Filter.Eq("name", "test")
            };
            var databases = await mongoClient.ListDatabasesAsync(options);

            Assert.True(databases.Any());
        }

        [Fact]
        public async Task DeveRetornarRegistro()
        {
            var response = await _client.GetAsync($"WeatherForecast/mongo/{2}");
            var content = await response.Content.ReadFromJsonAsync<WeatherForecast>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, content?.Id ?? 0);
        }
    }
}
