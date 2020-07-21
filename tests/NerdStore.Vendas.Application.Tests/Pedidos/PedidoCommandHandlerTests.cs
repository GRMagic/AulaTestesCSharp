using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido Com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async void AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IPedidoRepository>().Setup(r => r.UoW.Commit()).Returns(Task.FromResult(true));

            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

            var adicionarItemPedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto A", 2, 100);

            // Act
            var result = pedidoHandler.Handle(adicionarItemPedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(await result);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()));
            mocker.GetMock<IPedidoRepository>().Verify(r => r.UoW.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None));

        }

        [Fact(DisplayName = "Adicionar Novo Item Pedido Rascunho com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async void AdicionarItem_NovoItemPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var itemExistente = new PedidoItem(Guid.NewGuid(), "Produto Existente", 1, 3);
            pedido.AdicionarItem(itemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(pedido.ClienteId, Guid.NewGuid(), "Produto Novo", 2, 9);
            
            var mocker = new AutoMocker();
            mocker.GetMock<IPedidoRepository>().Setup(r => r.ObterPedidoRascunhoPorClienteId(pedido.ClienteId)).Returns(Task.FromResult(pedido));
            mocker.GetMock<IPedidoRepository>().Setup(r => r.UoW.Commit()).Returns(Task.FromResult(true));

            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

            // Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()));
            mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()));
            mocker.GetMock<IPedidoRepository>().Verify(r => r.UoW.Commit(), Times.Once);

        }

        [Fact(DisplayName = "Adicionar Item Existente Pedido Rascunho com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async void AdicionarItem_ItemExistentePedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(clienteId);

            var pedidoItemExistente = new PedidoItem(produtoId, "Nome do Produto", 1, 2);
            pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId, produtoId, "Nome do Produto", 1, 2);

            var mocker = new AutoMocker();
            mocker.GetMock<IPedidoRepository>().Setup(r => r.ObterPedidoRascunhoPorClienteId(pedido.ClienteId)).Returns(Task.FromResult(pedido));
            mocker.GetMock<IPedidoRepository>().Setup(r => r.UoW.Commit()).Returns(Task.FromResult(true));

            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

            // Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.UoW.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            var mocker = new AutoMocker();
            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

            // Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
