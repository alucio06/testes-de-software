﻿using Xunit;

namespace Features.Tests._02___Fixtures
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteTesteValido
    {
        readonly ClienteTestsFixture _clienteTestsFixture;

        public ClienteTesteValido(ClienteTestsFixture clienteTestsFixture)
        {
            _clienteTestsFixture = clienteTestsFixture;
        }

        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Fixture Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            // Arrange
            var cliente = _clienteTestsFixture.GerarClienteValido();

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.True(result);
            Assert.Empty(cliente.ValidationResult.Errors);
        }
    }


}
