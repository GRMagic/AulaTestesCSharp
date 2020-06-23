using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            ValidarQuantidade(quantidade);

            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        internal void AdicionarUnidades(int unidades)
        {
            var novaQuantidade = Quantidade + unidades;
            ValidarQuantidade(novaQuantidade);
            Quantidade = novaQuantidade;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        private void ValidarQuantidade(int quantidade)
        {
            if (quantidade > Pedido.MAX_UNIDADES_ITEM) throw new DomainException($"O limite máximo de {Pedido.MAX_UNIDADES_ITEM} unidades foi exedido.");
            if (quantidade < Pedido.MIN_UNIDADES_ITEM) throw new DomainException($"O limite mínimo de {Pedido.MIN_UNIDADES_ITEM} unidades não foi atendido.");
        }
    }
}
