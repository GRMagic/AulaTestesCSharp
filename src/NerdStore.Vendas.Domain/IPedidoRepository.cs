using NerdStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void Adicionar(Pedido pedido);

        void Atualizar(Pedido pedido);

        public Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);

        Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clienteId);

        void AdicionarItem(PedidoItem item);

        void AtualizarItem(PedidoItem item);

        void RemoverItem(PedidoItem pedidoItem);

        Task<Voucher> ObterVoucherPorCodigo(string codigo);

        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
    }
}
