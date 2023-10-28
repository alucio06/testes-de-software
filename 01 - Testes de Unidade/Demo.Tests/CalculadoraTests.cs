using Xunit;

namespace Demo.Tests
{
    public class CalculadoraTests
    {
        [Fact]
        public void Calculadora_Somar_RetornarValorSoma()
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(3, 4);

            // Assert
            Assert.Equal(7, resultado);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(3, 2, 5)]
        [InlineData(10, 5, 15)]
        [InlineData(9, 9, 18)]
        public void Calculadora_Somar_RetornarValoresSomaCorretos(double v1, double v2, double total)
        {
            //Arrange
            var calculadora = new Calculadora();

            // Act
            var result = calculadora.Somar(v1, v2);

            // Assert
            Assert.Equal(total, result);
        }
    }
}
