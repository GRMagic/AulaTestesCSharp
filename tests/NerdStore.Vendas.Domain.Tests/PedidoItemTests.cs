using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Adicionar item pedido acima do limite de unidades")]
        [Trait("Categoria", "Pedido Item Tests")]
        public void AdicionarItemPedido_ItemAcimaDoLimiteDeUnidades_DeveRetornarException()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Nome Produto", Pedido.MAX_UNIDADES_ITEM + 1, 100));
        }

        [Fact(DisplayName = "Adicionar item pedido abaixo do limite de unidades")]
        [Trait("Categoria", "Pedido Item Tests")]
        public void AdicionarItemPedido_ItemAbaixoDoLimiteDeUnidades_DeveRetornarException()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Nome Produto", Pedido.MIN_UNIDADES_ITEM - 1, 100));
        }
    }
}
