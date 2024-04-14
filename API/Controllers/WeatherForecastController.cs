using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ContextoBanco contexto) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken)
        {
            return await contexto.Set<WeatherForecast>().ToArrayAsync(cancellationToken);
        }


        [HttpGet("{id}")]
        public async Task<WeatherForecast?> GetById(int id, CancellationToken cancellationToken)
        {
            return await contexto.Set<WeatherForecast>().FindAsync([id], cancellationToken: cancellationToken);
        }

        [HttpGet("mongo/{id}")]
        public async Task<WeatherForecast?> GetByMongo(
            [FromServices] MongoContexto mongo,
            [FromRoute] int id)
        {
            return await mongo.Get(id);
        }
    }
}
