using Features.Tests._04___Dados_Humanos;
using FluentAssertions;
using Xunit;

namespace Features.Tests._07___FluentAssertions
{
    [Collection(nameof(ClienteBogusCollection))]
    public class ClienteFluentAssertionsTests
    {
        readonly ClienteTestsBogusFixture _clienteTestsBogusFixture;

        public ClienteFluentAssertionsTests(ClienteTestsBogusFixture clienteTestsBogusFixture)
        {
            _clienteTestsBogusFixture = clienteTestsBogusFixture;
        }

        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente FluentAssertions Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            // Arrange
            var cliente = _clienteTestsBogusFixture.GerarClienteValido();

            //Act
            var result = cliente.EhValido();

            //Assert
            //Assert.True(result);
            //Assert.Empty(cliente.ValidationResult.Errors);

            //Assert (FluentValidation)
            result.Should().BeTrue();
            cliente.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente FluentAssertions Testes")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            //Arrange
            var cliente = _clienteTestsBogusFixture.GerarClienteInvalido();

            //Act
            var result = cliente.EhValido();

            //Assert
            //Assert.False(result);
            //Assert.NotEmpty(cliente.ValidationResult.Errors);

            //Assert (FluentValidation)
            result.Should().BeFalse();
            cliente.ValidationResult.Errors.Should().HaveCountGreaterThanOrEqualTo(0, "deve possuir erros de validação");
        }

    }
}
