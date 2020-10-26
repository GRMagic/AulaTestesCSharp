using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.BDD.Tests.Login
{
    public class LoginUsuarioTela : BaseUsuarioTela
    {
        public LoginUsuarioTela(SeleniumHelper helper) : base(helper) { }

        public void ClicarNoLinkLogin()
        {
            Helper.ClicarLinkPorTexto("Login");
        }

        public void PreencherFormularioLogin(Usuario.Usuario usuario)
        {
            Helper.PreencherTextBoxPorId("Input_Email", usuario.Email);
            Helper.PreencherTextBoxPorId("Input_Password", usuario.Senha);
        }

        public bool ValidarPreenchimentoFormularioLogin(Usuario.Usuario usuario)
        {
            if (Helper.ObterValorTextBoxPorId("Input_Email") != usuario.Email) return false;
            if (Helper.ObterValorTextBoxPorId("Input_Password") != usuario.Senha) return false;

            return true;
        }

        public void ClicarNoBotaoLogin()
        {
            var botao = Helper.ObterElementoPorXPath("//*[@id='account']/div[5]/button");
            botao.Click();
        }

        public bool Login(Usuario.Usuario usuario)
        {
            AcessarSiteLoja();
            ClicarNoLinkLogin();
            PreencherFormularioLogin(usuario);
            if (!ValidarPreenchimentoFormularioLogin(usuario)) return false;
            ClicarNoBotaoLogin();
            if (!ValidarSaudacaoUsuarioLogado(usuario)) return false;

            return true;
        }
    }
}
