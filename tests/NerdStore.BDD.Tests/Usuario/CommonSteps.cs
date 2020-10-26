using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    public abstract class CommonSteps
    {
        protected BaseUsuarioTela _baseUsuarioTela;
        private readonly AutomacaoWebTestsFixture _automacaoWebTestsFixture;

        public CommonSteps(AutomacaoWebTestsFixture automacaoWebTestsFixture)
        {
            _automacaoWebTestsFixture = automacaoWebTestsFixture;
        }

        [Given(@"Que o visitante está acessando o site da loja")]
        public void DadoQueOVisitanteEstaAcessandoOSiteDaLoja()
        {
            // Act
            _baseUsuarioTela.AcessarSiteLoja();

            // Assert
            Assert.Contains(_automacaoWebTestsFixture.Configuration.DomainUrl, _baseUsuarioTela.ObterUrl());
        }

        [Then(@"Uma saudação com seu e-mail será exibida no menu superior")]
        public void EntaoUmaSaudacaoComSeuE_MailSeraExibidaNoMenuSuperior()
        {
            // Assert
            Assert.True(_baseUsuarioTela.ValidarSaudacaoUsuarioLogado(_automacaoWebTestsFixture.Usuario));
        }

        [Then(@"Ele será redirecionado para a vitrine")]
        public void EntaoEleSeraRedirecionadoParaAVitrine()
        {
            // Assert
            Assert.Contains(_automacaoWebTestsFixture.Configuration.VitrineUrl, _baseUsuarioTela.ObterUrl());
        }
    }
}
