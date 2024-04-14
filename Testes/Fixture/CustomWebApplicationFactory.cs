using API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Testes.Fixture
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private HttpClient? _httpClient;

        public HttpClient HttpClient => ProverClient();

        private HttpClient ProverClient()
        {
            if (_httpClient is not null) return _httpClient;

            // CreateClient cria uma instancia do Program.cs da API.
            _httpClient = CreateClient();

            // A execução dos scripts na base de dados deve ser feita após a inicialização do
            // client da API, visto que, será durante a inicialização do client que a migração
            // do banco será executada.
            MsSqlConfigurationProvider.RunScripts();
            MongoConfigurationProvider.RunScripts();

            return _httpClient;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Development");

            builder.ConfigureAppConfiguration(configure =>
            {
                configure.Add(new MongoConfigurationSource());
                configure.Add(new MsSqlConfigurationSource());
                configure.Add(new RabbitMqConfigurationSource());
                //configure.Add(new RedisConfigurationSource());
            });
        }
    }
}
