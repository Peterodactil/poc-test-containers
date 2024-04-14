using Microsoft.Extensions.Configuration;
using Testcontainers.Redis;

namespace Testes.Fixture
{
    public sealed class RedisConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RedisConfigurationProvider();
        }
    }

    public sealed class RedisConfigurationProvider : ConfigurationProvider
    {
        private static RedisContainer? _container;

        public static RedisContainer Container => _container!;

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
            _container = new RedisBuilder()
                .WithImage(Constantes.DockerImagens.MongoDb)
                .Build();

            await _container.StartAsync().ConfigureAwait(false);
        }
    }
}
