using Features.Clientes;
using Xunit;

namespace Features.Tests._01___Traits
{
    public class ClienteTests
    {
        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Trait Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(), "Anderson", "Lúcio", DateTime.Now.AddYears(-30),
                "alucio2106@gmail.com", true, DateTime.Now);

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.True(result);
            Assert.Empty(cliente.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente Trait Testes")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(), "", "", DateTime.Now,
                "teste.com", true, DateTime.Now);

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.False(result);
            Assert.NotEmpty(cliente.ValidationResult.Errors);
        }

    }
}
