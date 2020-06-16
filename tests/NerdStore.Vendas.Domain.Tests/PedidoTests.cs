using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar item novo pedido")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_PedidoNovo_DeveAtualizarValor()
        {
            // Arrange
            var pedido = new Pedido();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Nome Produto", 2, 100);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(200, pedido.ValorTotal);

        }
    }
}
