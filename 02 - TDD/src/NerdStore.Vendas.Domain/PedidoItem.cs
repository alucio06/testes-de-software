namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; } = string.Empty;
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            if (quantidade < Pedido.MIN_UNIDADES_ITEM) throw new DomainException($"Mínimo de {Pedido.MIN_UNIDADES_ITEM} por produto");

            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        internal void AdicionarQuantidade(int quantidade)
        {
            if (quantidade + Quantidade > Pedido.MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {Pedido.MAX_UNIDADES_ITEM} por produto");
            Quantidade += quantidade;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}
