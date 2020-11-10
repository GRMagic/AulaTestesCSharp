using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.Usuario;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Login
{
    [Binding]
    [Scope(Feature = "Usuário - Login")]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class LoginDeUsuarioSteps : CommonSteps
    {

        private readonly LoginUsuarioTela _loginUsuarioTela;
        private readonly AutomacaoWebTestsFixture _automacaoWebTestsFixture;

        public LoginDeUsuarioSteps(AutomacaoWebTestsFixture automacaoWebTestsFixture) : base(automacaoWebTestsFixture)
        {
            _automacaoWebTestsFixture = automacaoWebTestsFixture;
            _baseUsuarioTela = _loginUsuarioTela = new LoginUsuarioTela(automacaoWebTestsFixture.BrowserHelper);
        }

        [When(@"Ele clicar em login")]
        public void QuandoEleClicarEmLogin()
        {
            // Act
            _loginUsuarioTela.ClicarNoLinkLogin();

            // Assert
            Assert.Contains(_automacaoWebTestsFixture.Configuration.LoginUrl,_loginUsuarioTela.ObterUrl());
        }        

        [When(@"Preencher os dados do formulario de login")]
        public void QuandoPreencherOsDadosDoFormularioDeLogin(Table table)
        {
            // Arrange
            var usuario = new Usuario.Usuario
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };
            _automacaoWebTestsFixture.Usuario = usuario;

            // Act
            _loginUsuarioTela.PreencherFormularioLogin(usuario);

            // Assert
            Assert.True(_loginUsuarioTela.ValidarPreenchimentoFormularioLogin(usuario));
        }
        
        [When(@"Clicar no botão login")]
        public void QuandoClicarNoBotaoLogin()
        {
            _loginUsuarioTela.ClicarNoBotaoLogin();
        }
    }
}
