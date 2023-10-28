using Features.Clientes;
using Xunit;

namespace Features.Tests._02___Fixtures
{
    [CollectionDefinition(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteTestsFixture>
    {

    }

    public class ClienteTestsFixture : IDisposable
    {
        public Cliente GerarClienteValido()
        {
            return new Cliente(
                Guid.NewGuid(),
                "Anderson",
                "Lúcio",
                DateTime.Now.AddYears(-30),
                "alucio2106@gmail.com",
                true,
                DateTime.Now);
        }

        public Cliente GerarClienteInvalido()
        {
            return new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "teste.com",
                true,
                DateTime.Now);
        }

        public void Dispose()
        {
        }
    }
}
