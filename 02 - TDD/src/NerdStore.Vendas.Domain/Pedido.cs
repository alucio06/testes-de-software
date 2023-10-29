using FluentValidation.Results;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;

        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; private set; }
        public decimal Desconto { get; set; }
        public PedidoStatus PedidoStatus { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public Voucher? Voucher { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplicavel();
            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularValorTotalDesconto();

            return result;
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado || Voucher == null) return;

            decimal desconto = 0;

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Valor)
            {
                desconto = Voucher.ValorDesconto.GetValueOrDefault();
            }

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem)
            {
                desconto = (ValorTotal * Voucher.PercentualDesconto.GetValueOrDefault()) / 100;
            }

            ValorTotal -= desconto > ValorTotal ? ValorTotal : desconto;
            Desconto = desconto;
        }

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            ValidarQuantidadeItemPermitida(pedidoItem);

            if (PedidoItemExistente(pedidoItem))
            {
                var itemExistente = _pedidoItems.First(p => p.ProdutoId == pedidoItem.ProdutoId);

                itemExistente.AdicionarQuantidade(pedidoItem.Quantidade);
                pedidoItem = itemExistente;
                _pedidoItems.Remove(itemExistente);
            }

            _pedidoItems.Add(pedidoItem);
            CalcularValorPedido();
        }

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);
            ValidarQuantidadeItemPermitida(pedidoItem);

            var itemExistente = _pedidoItems.First(p => p.ProdutoId == pedidoItem.ProdutoId);
            
            _pedidoItems.Remove(itemExistente);
            _pedidoItems.Add(pedidoItem);

            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);

            _pedidoItems.Remove(pedidoItem);

            CalcularValorPedido();
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem item)
        {
            var quantidadeItems = item.Quantidade;

            if (PedidoItemExistente(item))
            {
                var itemExistente = _pedidoItems.First(p => p.ProdutoId == item.ProdutoId);
                quantidadeItems += itemExistente.Quantidade;
            }

            if (quantidadeItems > MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} por produto");
        }

        private bool PedidoItemExistente(PedidoItem item)
        {
            return _pedidoItems.Any(p => p.ProdutoId == item.ProdutoId);
        }

        private void ValidarPedidoItemInexistente(PedidoItem item)
        {
            if (!PedidoItemExistente(item)) throw new DomainException("Item não existe no pedido");
        }

        private void CalcularValorPedido()
        {
            ValorTotal = _pedidoItems.Sum(i => i.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
}