using NerdStore.BDD.Tests.Config;
using System;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Pedido
{
    [Binding]
    public class Pedido_AdicionarItemAoCarrinhoSteps
    {
        [Given(@"que um produto esteja na vitrine")]
        public void DadoQueUmProdutoEstejaNaVitrine()
        {
            // Arrange
            var browser = new SeleniumHelper(Browser.Chrome, new ConfigurationHelper(), false);
            browser.IrParaUrl("https://google.com.br");
            // Act

            // Assert
        }
        
        [Given(@"esteja disponível no estoque")]
        public void DadoEstejaDisponivelNoEstoque()
        {
            // Arrange

            // Act

            // Assert
        }

        [Given(@"o usuário esteja logado")]
        public void DadoOUsuarioEstejaLogado()
        {
            // Arrange

            // Act

            // Assert
        }

        [Given(@"o mesmo produto tenha sido adicionado ao carrinho anteriormente")]
        public void DadoOMesmoProdutoTenhaSidoAdicionadoAoCarrinhoAnteriormente()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"o usuário adicionar uma unidade ao carrinho")]
        public void QuandoOUsuarioAdicionarUmaUnidadeAoCarrinho()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"o usuário adicionar um item acima da quantidade máxima permitida")]
        public void QuandoOUsuarioAdicionarUmItemAcimaDaQuantidadeMaximaPermitida()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"o usuário adicionar uma unidade no pedido")]
        public void QuandoOUsuarioAdicionarUmaUnidadeNoPedido()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"o usuário adicionar mais que a quantidade máxima permitida ao carrinho")]
        public void QuandoOUsuarioAdicionarMaisQueAQuantidadeMaximaPermitidaAoCarrinho()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"o usuário será redirecionado ao resumo da compra")]
        public void EntaoOUsuarioSeraRedirecionadoAoResumoDaCompra()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"o valor total do pedido será extatamente o valor do item adicionado")]
        public void EntaoOValorTotalDoPedidoSeraExtatamenteOValorDoItemAdicionado()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite")]
        public void EntaoReceberaUmaMensagemDeErroMencionandoQueFoiUltrapassadaAQuantidadeLimite()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"a quantidade quantidade daquele produto será acrescida de uma unidade a mais")]
        public void EntaoAQuantidadeQuantidadeDaqueleProdutoSeraAcrescidaDeUmaUnidadeAMais()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"o valor total do pedido será a multiplicação da quantidade de itens pelo valor unitário")]
        public void EntaoOValorTotalDoPedidoSeraAMultiplicacaoDaQuantidadeDeItensPeloValorUnitario()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
