using NerdStore.WebApp.MVC;
using NerdStore.WebApp.MVC.Models;
using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApp.Tests.Features;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("NerdStore.WebApp.Tests.Features.PriorityOrderer", "NerdStore.WebApp.Tests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class PedidoApiTests
    {

        private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;

        public PedidoApiTests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(1)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("113A4952-FAFD-45CC-B0B6-197D3C5F51A4"),
                Quantidade = 2
            };

            await _testsFixture.RealizarLoginApi();
            
            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", itemInfo);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(3)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task RemoverItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("113A4952-FAFD-45CC-B0B6-197D3C5F51A4");

            await _testsFixture.RealizarLoginApi();
            
            // Act
            var deleteResponse = await _testsFixture.Client.DeleteAsync($"api/carrinho/{produtoId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Atualizar item em pedido existente"), TestPriority(2)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task AtualizarItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("113A4952-FAFD-45CC-B0B6-197D3C5F51A4"),
                Quantidade = 1
            };

            await _testsFixture.RealizarLoginApi();

            // Act
            var deleteResponse = await _testsFixture.Client.PutAsJsonAsync($"api/carrinho/{itemInfo.Id}", itemInfo);

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}
