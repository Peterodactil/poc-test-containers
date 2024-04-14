using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

namespace Testes.TestContainersExtensions
{
    public class UntilTimeDelay(int millisecondsDelay = 3_000) : IWaitUntil
    {
        public Task<bool> UntilAsync(IContainer container)
        {
            Task.Delay(millisecondsDelay).Wait();
            return Task.FromResult(true);
        }
    }
}
