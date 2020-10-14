using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class CommonSteps
    {
        private readonly CadastroDeUsuarioTela _cadastroDeUsuarioTela;
        private readonly AutomacaoWebTestsFixture _automacaoWebTestsFixture;

        public CommonSteps(CadastroDeUsuarioTela cadastroDeUsuarioTela, AutomacaoWebTestsFixture automacaoWebTestsFixture)
        {
            _cadastroDeUsuarioTela = cadastroDeUsuarioTela;
            _automacaoWebTestsFixture = automacaoWebTestsFixture;
        }

        [Given(@"Que o visitante está acessando o site da loja")]
        public void DadoQueOVisitanteEstaAcessandoOSiteDaLoja()
        {
            // Act
            _cadastroDeUsuarioTela.AcessarSiteLoja();

            // Assert
            Assert.Contains(_automacaoWebTestsFixture.Configuration.DomainUrl, _cadastroDeUsuarioTela.ObterUrl());
        }

        [Then(@"Uma saudação com seu e-mail será exibida no menu superior")]
        public void EntaoUmaSaudacaoComSeuE_MailSeraExibidaNoMenuSuperior()
        {
            // Assert
            Assert.True(_cadastroDeUsuarioTela.ValidarSaudacaoUsuarioLogado(_automacaoWebTestsFixture.Usuario));
        }

        [Then(@"Ele será redirecionado para a vitrine")]
        public void EntaoEleSeraRedirecionadoParaAVitrine()
        {
            // Assert
            Assert.Contains(_automacaoWebTestsFixture.Configuration.VitrineUrl, _cadastroDeUsuarioTela.ObterUrl());
        }
    }
}
