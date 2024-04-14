using Testes.Fixture;

namespace Testes
{
    [CollectionDefinition("API")]
    public class ApiCollection : ICollectionFixture<CustomWebApplicationFactory>;
}
