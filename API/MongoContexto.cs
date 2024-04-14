using MongoDB.Driver;

namespace API
{
    public class MongoContexto(IConfiguration configuration)
    {
        private readonly MongoClient client = new(configuration.GetConnectionString("MongoDb"));

        public async Task<WeatherForecast> Get(int id)
        {
            var filter = Builders<WeatherForecast>.Filter.Eq("Id", id);

            var resultado = await client.GetDatabase("test")
                .GetCollection<WeatherForecast>("climas")
                .FindAsync<WeatherForecast>(filter);

            return resultado.FirstOrDefault();
        }
    }
}
