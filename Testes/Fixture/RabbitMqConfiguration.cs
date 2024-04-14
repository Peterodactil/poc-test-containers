using Microsoft.Extensions.Configuration;
using Testcontainers.RabbitMq;

namespace Testes.Fixture
{
    public sealed class RabbitMqConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RabbitMqConfigurationProvider();
        }
    }

    public sealed class RabbitMqConfigurationProvider : ConfigurationProvider
    {
        private static RabbitMqContainer? _container;

        public static RabbitMqContainer Container => _container!;

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
            _container = new RabbitMqBuilder()
                .WithImage(Constantes.DockerImagens.RabbitMq)
                .Build();

            await _container.StartAsync().ConfigureAwait(false);
        }
    }
}
