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

    }
}
