using AngleSharp.Html.Parser;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class PedidoWebTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;
        public PedidoWebTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Adicionar Item em Novo Pedido")]
        [Trait("Categoria", "Integração Web - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveAtualizarValorTotal()
        {
            // Arrange

            await _testsFixture.RealizarLoginWeb();

            var produtoId = new Guid("113A4952-FAFD-45CC-B0B6-197D3C5F51A4");
            const int quantidade = 2;
            
            var inicialResponse = await _testsFixture.Client.GetAsync($"/produto-detalhe/{produtoId}");
            inicialResponse.EnsureSuccessStatusCode();

            var formData = new Dictionary<string, string>()
            {
                { "Id", produtoId.ToString() },
                { "quantidade", quantidade.ToString() }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "meu-carrinho")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            var html = (await new HtmlParser().ParseDocumentAsync(await postRequest.Content.ReadAsStringAsync())).All;

            var formQuantidade = html?.FirstOrDefault(e => e.Id == "quantidade")?.GetAttribute("value");

        }
    }
}
