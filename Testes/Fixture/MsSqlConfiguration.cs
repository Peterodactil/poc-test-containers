using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace Testes.Fixture
{
    public sealed class MsSqlConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MsSqlConfigurationProvider();
        }
    }

    public sealed class MsSqlConfigurationProvider : ConfigurationProvider
    {
        private static MsSqlContainer? _container;

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
            _container = new MsSqlBuilder()
                .WithImage(Constantes.DockerImagens.MsSqlServe)
                //.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();

            await _container.StartAsync().ConfigureAwait(false);

            Set(Constantes.AppSettingsChaves.MsSqlConnectionString, _container.GetConnectionString());
        }

        public static void RunScripts()
        {
            if (_container is null) throw new Exception("Container MsSqlServer não iniciado.");

            foreach (string arquivo in Directory.EnumerateFiles("./Scripts", "*.sql"))
            {
                var comando = File.ReadAllText(arquivo);

                _container.ExecScriptAsync(comando)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
