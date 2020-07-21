using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!await ValidarComando(message, cancellationToken)) return false;

            var pedidoItem = new PedidoItem(message.ProdutoId, message.ProdutoNome, message.Quantidade, message.ValorUnitario);

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            if(pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);
                _pedidoRepository.Adicionar(pedido);
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);
                if (pedidoItemExistente)
                {
                    _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == message.ProdutoId));
                }
                else
                {    
                    _pedidoRepository.AdicionarItem(pedidoItem);
                }
                _pedidoRepository.Atualizar(pedido);
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, pedidoItem.ProdutoId, pedidoItem.ProdutoNome, pedidoItem.ValorUnitario, pedidoItem.Quantidade));
            
            return await _pedidoRepository.UoW.Commit();
        }

        private async Task<bool> ValidarComando(Command command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                foreach (var error in command.ValidationResult.Errors)
                {
                    await _mediator.Publish(new DomainNotification(command.Type, error.ErrorMessage), cancellationToken);
                }
                return false;
            }

            return true;
        }
    }
}
