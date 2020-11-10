﻿using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.Login;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Pedido
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class Pedido_AdicionarItemAoCarrinhoSteps
    {
        private readonly AutomacaoWebTestsFixture _testsFixture;
        private readonly PedidoTela _pedidoTela;
        private readonly LoginUsuarioTela _loginUsuarioTela;

        private string _urlProduto;

        public Pedido_AdicionarItemAoCarrinhoSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _pedidoTela = new PedidoTela(testsFixture.BrowserHelper);
            _loginUsuarioTela = new LoginUsuarioTela(testsFixture.BrowserHelper);
        }

        [Given(@"que o usuário esteja logado")]
        public void DadoQueOUsuarioEstejaLogado()
        {
            // Arrange
            _testsFixture.Usuario = new Usuario.Usuario
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };

            // Act
            var logado = _loginUsuarioTela.Login(_testsFixture.Usuario);

            // Assert
            Assert.True(logado);
        }

        [Given(@"que um produto esteja na vitrine")]
        public void DadoQueUmProdutoEstejaNaVitrine()
        {
            // Arrange
            _pedidoTela.AcessarVitrineDeProdutos();

            // Act
            _pedidoTela.ObterDetalhesProduto();
            _urlProduto = _pedidoTela.ObterUrl();

            // Assert
            Assert.True(_pedidoTela.ValidarUrlProduto());

        }
        
        [Given(@"esteja disponível no estoque")]
        public void DadoEstejaDisponivelNoEstoque()
        {
            // Assert
            Assert.True(_pedidoTela.ObterQuantidadeNoEstoque() > 0);
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
            // Act
            _pedidoTela.ClicarEmComprarAgora();
        }

        [When(@"o usuário adicionar um item acima da quantidade máxima permitida")]
        public void QuandoOUsuarioAdicionarUmItemAcimaDaQuantidadeMaximaPermitida()
        {
            // Arrange
            _pedidoTela.ClicarAdicionarQuantidadeItens(Vendas.Domain.Pedido.MAX_UNIDADES_ITEM + 1);

            // Act
            _pedidoTela.ClicarEmComprarAgora();
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
            // Assert
            Assert.True(_pedidoTela.ValidarSeEstaNoCarrinhoDeCompras());
        }

        [Then(@"o valor total do pedido será extatamente o valor do item adicionado")]
        public void EntaoOValorTotalDoPedidoSeraExtatamenteOValorDoItemAdicionado()
        {
            // Arrange
            var valorUnitario = _pedidoTela.ObterValorUnitarioProdutoCarrinho();
            var valorTotal = _pedidoTela.ObterValorTotalCarrinho();

            // Assert
            Assert.Equal(valorUnitario, valorTotal);
        }

        [Then(@"receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite")]
        public void EntaoReceberaUmaMensagemDeErroMencionandoQueFoiUltrapassadaAQuantidadeLimite()
        {
            // Arrange
            var mensagem = _pedidoTela.ObterMensagemDeErroProduto();

            // Assert
            Assert.Contains(Vendas.Domain.Pedido.MAX_UNIDADES_ITEM.ToString(), mensagem);
        }


        [Given(@"aceita o uso de cookies")]
        public void EAceitaUsoDeCookies()
        {
            // Act
            _loginUsuarioTela.AceitarCookies();

            // Assert
            Assert.False(_loginUsuarioTela.ValidarSeElementoExistePorId("#cookieConsent"));
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
