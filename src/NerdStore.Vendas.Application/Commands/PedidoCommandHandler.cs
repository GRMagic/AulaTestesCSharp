using MediatR;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.ProdutoNome, message.Quantidade, message.ValorUnitario);
            pedido.AdicionarItem(pedidoItem);

            _pedidoRepository.Adicionar(pedido);
            
            await _mediator.Publish(new PedidoItemAdicionadoEvent(pedido.ClienteId,pedido.Id, pedidoItem.ProdutoId, pedidoItem.ProdutoNome, pedidoItem.ValorUnitario, pedidoItem.Quantidade), cancellationToken);
            return true;
        }
    }
}
