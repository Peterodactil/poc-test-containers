using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddScoped<ContextoBanco>();
            builder.Services.AddScoped<MongoContexto>();

            BsonClassMap.RegisterClassMap<WeatherForecast>(cm =>
            {
                cm.AutoMap();
                cm.MapIdField(c => c.Id);
                cm.MapField(c => c.Date).SetSerializer(new DateTimeSerializer(dateOnly: true));
            });

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var contexto = scope.ServiceProvider.GetService<ContextoBanco>();
            contexto!.Database.EnsureCreated();

            app.Run();
        }
    }
}
