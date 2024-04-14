namespace Testes
{
    public static class Constantes
    {
        public static class DockerImagens
        {
            public const string MongoDb = "mongo:4.2";
            public const string MsSqlServe = "mcr.microsoft.com/mssql/server:2017-latest";
            public const string RabbitMq = "rabbitmq:3-management-alpine";
            public const string Redis = "redis:latest";
        }

        public static class AppSettingsChaves
        {
            public const string MongoConnectionString = "ConnectionStrings:MongoDb";
            public const string MsSqlConnectionString = "ConnectionStrings:DefaultConnection";
        }
    }
}
