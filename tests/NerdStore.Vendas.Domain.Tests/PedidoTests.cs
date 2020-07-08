using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar item novo pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_PedidoNovo_DeveAtualizarValor()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Nome Produto", 2, 100);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(200, pedido.ValorTotal);

        }

        [Fact(DisplayName = "Adicionar item existente pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoID = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoID, "Nome Produto", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoID, "Nome Produto", 1, 100);

            // Act
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == produtoID)?.Quantidade);
        }

        [Fact(DisplayName = "Adicionar item existente pedido quantidade acima permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteMaximo_DeveLancarExeption()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoID = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoID, "Nome Produto", Pedido.MAX_UNIDADES_ITEM, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoID, "Nome Produto", 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExisteNaLista_DeveLancarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Nome Produto", 5, 10);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Nome Produto", 1, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Nome Produto", 2, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == produtoId)?.Quantidade);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 15);
            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                              pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        //[Fact(DisplayName = "Atualizar Item Pedido Quantidade acima do permitido")]
        //[Trait("Categoria", "Vendas - Pedido")]
        //public void AtualizarItemPedido_ItemUnidadesAcimaDoPermitido_DeveRetornarException()
        //{
        //    // Arrange
        //    var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        //    var produtoId = Guid.NewGuid();
        //    var pedidoItemExistente1 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
        //    pedido.AdicionarItem(pedidoItemExistente1);

        //    var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 15); // <== Ao criar um item com a quantidade fora dos limites já é lançada uma exceção. A própria classe não permite ser criada de forma inválida, e não permite alteração da quantidade.

        //    // Act & Assert
        //    Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        //}

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoExiste_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Item Inexistente", 1, 10);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));
        }

        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExiste_DeveAtualizarValorPedido()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario;

            // Act
            pedido.RemoverItem(pedidoItemExistente2);

            // Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("10-PILA", null, 10, 5, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);

            // Act
            var result = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher Inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarSemErros()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("10-PILA", null, 10, 5, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), true, false);

            // Act
            var result = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher Tipo Valor Desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValor_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "Produto A", 1, 10));
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "Produto B", 2, 20));
            var voucher = new Voucher("10-PILA", null, 10, 5, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);
            var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;


            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Tipo Percentual Desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentual_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "Produto A", 1, 10));
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "Produto B", 2, 20));
            var voucher = new Voucher("10%OFF", 10, null, 5, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(1), true, false);
            var valorComDesconto = pedido.ValorTotal - pedido.ValorTotal * voucher.PercentualDesconto / 100;


            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Desconto Excede Valor Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ExcedeValorTotal_PedidoDeveTerValorZero()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "Produto A", 1, 200));
            var voucher = new Voucher("10%OFF", null, 300, 5, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(0, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Recalcular Desconto na Modificação do Pedido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void AplicarVoucher_ModificarItensPedido_DeveRecalcularValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "Produto A", 1, 200);
            pedido.AdicionarItem(item);
            var voucher = new Voucher("10", null, 10, 5, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);
            pedido.AplicarVoucher(voucher);

            var item2 = new PedidoItem(Guid.NewGuid(), "Produto B", 1, 10);

            // Act
            pedido.AdicionarItem(item2);

            // Assert
            var totalEsperado = pedido.PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalEsperado, pedido.ValorTotal);
        }
    }
}
