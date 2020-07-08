using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public decimal ValorTotal { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }
        public Guid ClienteId { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        private bool PedidoItemExistente(PedidoItem pedidoItem)
        {
            return _pedidoItems.Any(i => i.ProdutoId == pedidoItem.ProdutoId);
        }

        public bool VoucherUtilizado { get; private set; }
        public Voucher Voucher { get; private set; }

        public decimal Desconto { get; private set; }

        public void ValidarPedidoItemInexistente(PedidoItem pedidoItemAtualizado)
        {
            if (!PedidoItemExistente(pedidoItemAtualizado)) throw new DomainException("O Item não existe no pedido");
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if(PedidoItemExistente(pedidoItem))
            {
                var produtoExistente = _pedidoItems.First(i => i.ProdutoId == pedidoItem.ProdutoId);
                produtoExistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = produtoExistente;
                _pedidoItems.Remove(produtoExistente);
            }

            _pedidoItems.Add(pedidoItem);
            CalcularValorPedido();
        }

        private void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(i => i.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }

        public void AtualizarItem(PedidoItem pedidoItemAtualizado)
        {
            ValidarPedidoItemInexistente(pedidoItemAtualizado);
            

            var itemExistente = PedidoItems.FirstOrDefault(i => i.ProdutoId == pedidoItemAtualizado.ProdutoId);
            _pedidoItems.Remove(itemExistente);

            _pedidoItems.Add(pedidoItemAtualizado);

            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem pedidoItemRemover)
        {
            ValidarPedidoItemInexistente(pedidoItemRemover);
            _pedidoItems.Remove(pedidoItemRemover);
            CalcularValorPedido();
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplicavel();
            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularValorPedido();

            return result;
        }

        private void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if( Voucher.Tipo == TipoDescontoVoucher.Valor)
            {
                desconto = Voucher.ValorDesconto ?? 0;
            }
            else
            {
                desconto = ValorTotal * (Voucher.PercentualDesconto ?? 0) / 100;
                
            }
            valor -= desconto;
            ValorTotal = valor > 0 ? valor : 0;
            Desconto = desconto;
        }
    }
}
