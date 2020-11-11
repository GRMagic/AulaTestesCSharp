using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.BDD.Tests.Pedido
{
    public class PedidoTela : PageObjectModel
    {
        public PedidoTela(SeleniumHelper helper) : base(helper) { }

        public void AcessarVitrineDeProdutos()
        {
            Helper.IrParaUrl(Helper.Configuration.VitrineUrl);
        }

        public void ObterDetalhesProduto(int posicao = 1)
        {
            Helper.ClicarPorXPath($"/html/body/div/main/div/div/div[{posicao}]/span/a");
        }

        public bool ValidarUrlProduto()
        {
            return Helper.ValidarConteudoUrl(Helper.Configuration.ProdutoUrl);
        }

        public int ObterQuantidadeNoEstoque()
        {
            var elemento = Helper.ObterElementoPorXPath("/html/body/div/main/div/div/div[2]/p[1]");
            return elemento.Text.ApenasNumeros();
        }

        public void ClicarEmComprarAgora()
        {
            Helper.ClicarPorXPath("/html/body/div/main/div/div/div[2]/form/div[2]/button");
        }

        public bool ValidarSeEstaNoCarrinhoDeCompras()
        {
            return Helper.ValidarConteudoUrl(Helper.Configuration.CarrinhoUrl);
        }

        public decimal ObterValorUnitarioProdutoCarrinho()
        {
            var str = Helper.ObterTextoElementoPorId("valorUnitario")
                .Replace("R", string.Empty)
                .Replace("$", string.Empty)
                .Replace(".", string.Empty)
                .Trim();
            return Convert.ToDecimal(str);
        }

        public decimal ObterValorTotalCarrinho()
        {
            var str = Helper.ObterTextoElementoPorId("valorTotalCarrinho")
                .Replace("R", string.Empty)
                .Replace("$", string.Empty)
                .Replace(".", string.Empty)
                .Trim();
            return Convert.ToDecimal(str);
        }

        public void ClicarAdicionarQuantidadeItens(int quantidade = 1)
        {
            var botaoAdicionar = Helper.ObterElementoPorClasse("btn-plus");
            if (botaoAdicionar == null) return;

            for (var i = 1; i <= quantidade; i++)
            {
                botaoAdicionar.Click();
            }
        }

        public string ObterMensagemDeErroProduto()
        {
            return Helper.ObterTextoElementoPorClasseCss("alert-danger");
        }

        public void NavegarParaCarrinhoDeCompras()
        {
            Helper.ObterElementoPorXPath("/html/body/header/nav/div/div/ul/li[3]/a").Click();
        }

        public string ObterIdPrimeiroProdutoCarrinho()
        {
            return Helper.ObterElementoPorXPath("/html/body/div/main/div/div/div/table/tbody/tr[1]/td[1]/div/div/h4/a")
                .GetAttribute("href");
        }

        public void GarantirQueOPrimeiroItemDaVitrineEstejaAdicionado()
        {
            NavegarParaCarrinhoDeCompras();
            if (ObterValorTotalCarrinho() > 0) return;

            AcessarVitrineDeProdutos();
            ObterDetalhesProduto();
            ClicarEmComprarAgora();
        }

        public int ObterQuantidadeDeItensPrimeiroProdutoCarrinho()
        {
            return Convert.ToInt32(Helper.ObterValorTextBoxPorId("quantidade"));
        }

        public void VoltarNavegacao(int vezes = 1)
        {
            Helper.VoltarNavegacao(vezes);
        }

        public void ZerarCarrinhoDeCompras()
        {
            while (ObterValorTotalCarrinho() > 0)
            {
                Helper.ClicarPorXPath("/html/body/div/main/div/div/div/table/tbody/tr[1]/td[5]/form/button");
            }
        }
    }
}
