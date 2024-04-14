using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Configuration;
using Testcontainers.MongoDb;

namespace Testes.Fixture
{
    public sealed class MongoConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoConfigurationProvider();
        }
    }

    public sealed class MongoConfigurationProvider : ConfigurationProvider
    {
        private static MongoDbContainer? _container;

        public static MongoDbContainer Container => _container!;

        private static readonly TaskFactory taskFactory = new(
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

        public override void Load()
        {
            taskFactory.StartNew(LoadAsync)
                .Unwrap()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public async Task LoadAsync()
        {
            _container = new MongoDbBuilder()
                .WithImage(Constantes.DockerImagens.MongoDb)
                // Não fazer uso de nenhum estratégia, não está garantindo a conclusão da espera do container ficar diponível (waiter da lib entra em loop infinito)
                // A estratégia de aguardar pela porta 27017 estar disponível não está garantindo a execução do scripts
                //.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))

                // A estratégia usando o comando abaixo me pareceu a melhor opção, garante que o shell do mongo estará em execução para rodar os scripts
                .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("echo 'db.runCommand(\"ping\").ok' | mongo --quiet"))

                // A estratégia customizada por tempo/delay, funciona, porém é necessário acertar o tempo de espera. Cada máquina pode ser necessário ter um tempo diferente
                //.WithWaitStrategy(Wait.ForUnixContainer().AddCustomWaitStrategy(new UntilTimeDelay(2_000)))
                .Build();

            await _container.StartAsync().ConfigureAwait(false);

            Set(Constantes.AppSettingsChaves.MongoConnectionString, _container.GetConnectionString());
        }

        public static void RunScripts()
        {
            if (_container is null) throw new Exception("Container MongoDB não iniciado.");

            foreach (string arquivo in Directory.EnumerateFiles("./ScriptsMongo", "*.js"))
            {
                var comando = File.ReadAllText(arquivo);

                var resultado = _container.ExecScriptAsync(comando)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
