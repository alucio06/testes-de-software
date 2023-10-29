using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {

        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Já Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemJaExistente_DeveIncrementarUnidadesESomarValores()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            
            var pedidoItem2 = new PedidoItem(produtoId, "Produto teste", 3, 100);

            // Act
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(5, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId)?.Quantidade);
            Assert.Equal(500, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
        }

        [Fact(DisplayName = "Adicionar Item Existente acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", Pedido.MAX_UNIDADES_ITEM, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto teste", 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar Item Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemInexistente_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", Pedido.MAX_UNIDADES_ITEM, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItem));
        }

        [Fact(DisplayName = "Atualizar Item Valido Quantidade Superior a atual")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidadeSuperior()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", 5, 100);

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(pedidoItemAtualizado.Quantidade, pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == produtoId)?.Quantidade);
        }

        [Fact(DisplayName = "Atualizar Item Valido Quantidade Inferior a atual")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidadeInferior()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", 1, 100);

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(pedidoItemAtualizado.Quantidade, pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == produtoId)?.Quantidade);
        }

        [Fact(DisplayName = "Atualizar Item Valido Quantidade Igual a atual")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidadeIgual()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", 3, 100);

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(pedidoItemAtualizado.Quantidade, pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == produtoId)?.Quantidade);
        }

        [Fact(DisplayName = "Atualizar Item Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 300);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto teste", 5, 600);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", 3, 600);

            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                              pedidoItemAtualizado.Quantidade * pedidoItemExistente2.ValorUnitario;

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Item Quantidade Acima do Permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", Pedido.MAX_UNIDADES_ITEM, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Remover Item Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemInexistente_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", Pedido.MAX_UNIDADES_ITEM, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItem));
        }

        [Fact(DisplayName = "Remover Item Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemValido_DeveAtualizarValorTota()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 300);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto teste", 5, 600);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario;

            // Act
            pedido.RemoverItem(pedidoItemExistente2);

            // Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", 15, null, TipoDescontoVoucher.Valor, 1,
                DateTime.Now.AddDays(15), true, false);

            // Act
            var result = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("", null, null, TipoDescontoVoucher.Valor, 0,
               DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher tipo valor")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto teste 1", 2, 300);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto teste 2", 5, 600);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            decimal valorVoucher = 15;
            var voucher = new Voucher("PROMO-15-REAIS", valorVoucher, null, TipoDescontoVoucher.Valor, 1,
                DateTime.Now.AddDays(15), true, false);

            var valorTotalPedido = pedidoItem1.Quantidade * pedidoItem1.ValorUnitario +
                                   pedidoItem2.Quantidade * pedidoItem2.ValorUnitario -
                                   valorVoucher;

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorTotalPedido, pedido.ValorTotal);

        }

        [Fact(DisplayName = "Aplicar voucher tipo percentual")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentual_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto teste 1", 2, 300);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto teste 2", 5, 600);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            decimal percentualVoucher = 15;
            var voucher = new Voucher("PROMO-15%-OFF", null, percentualVoucher, TipoDescontoVoucher.Porcentagem, 1,
                DateTime.Now.AddDays(15), true, false);

            var valorTotalPedido = (pedidoItem1.Quantidade * pedidoItem1.ValorUnitario + pedidoItem2.Quantidade * pedidoItem2.ValorUnitario) *
                                   (1 - percentualVoucher/100);

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorTotalPedido, pedido.ValorTotal);

        }

        [Fact(DisplayName = "Aplicar voucher desconto excede valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto teste 1", 1, 12);
            pedido.AdicionarItem(pedidoItem1);

            decimal valorVoucher = 15;
            var voucher = new Voucher("PROMO-15-REAIS", valorVoucher, null, TipoDescontoVoucher.Valor, 1,
                DateTime.Now.AddDays(15), true, false);

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(0, pedido.ValorTotal);

        }

        [Fact(DisplayName = "Aplicar voucher recalcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto teste 1", 1, 100);
            pedido.AdicionarItem(pedidoItem1);

            decimal valorVoucher = 15;
            var voucher = new Voucher("PROMO-15-REAIS", valorVoucher, null, TipoDescontoVoucher.Valor, 1,
                DateTime.Now.AddDays(15), true, false);

            pedido.AplicarVoucher(voucher);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto teste 2", 1, 200);

            var valorTotalPedido = pedidoItem1.Quantidade * pedidoItem1.ValorUnitario +
                                   pedidoItem2.Quantidade * pedidoItem2.ValorUnitario -
                                   valorVoucher;

            // Act
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(valorTotalPedido, pedido.ValorTotal);

        }




    }
}
