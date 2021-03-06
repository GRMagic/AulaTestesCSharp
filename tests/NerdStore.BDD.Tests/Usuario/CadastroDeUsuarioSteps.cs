﻿using NerdStore.BDD.Tests.Config;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [Scope(Feature = "Usuário - Cadastro")]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class CadastroDeUsuarioSteps : CommonSteps
    {
        private readonly CadastroDeUsuarioTela _cadastroDeUsuarioTela;
        private readonly AutomacaoWebTestsFixture _automacaoWebTestsFixture;

        public CadastroDeUsuarioSteps(AutomacaoWebTestsFixture automacaoWebTestsFixture) : base(automacaoWebTestsFixture)
        {
            _automacaoWebTestsFixture = automacaoWebTestsFixture;
            _baseUsuarioTela = _cadastroDeUsuarioTela = new CadastroDeUsuarioTela(automacaoWebTestsFixture.BrowserHelper);
        }

        [When(@"Ele clicar em registrar")]
        public void QuandoEleClicarEmRegistrar()
        {
            // Act
            _cadastroDeUsuarioTela.ClicarNoLinkRegistrar();

            // Assert
            Assert.Contains(_automacaoWebTestsFixture.Configuration.RegisterUrl, _cadastroDeUsuarioTela.ObterUrl());
        }
        
        [When(@"Preencher os dados do formulario")]
        public void QuandoPreencherOsDadosDoFormulario(Table table)
        {
            // Arrange
            _automacaoWebTestsFixture.GerarDadosUsuario();
            var usuario = _automacaoWebTestsFixture.Usuario;

            // Act
            _cadastroDeUsuarioTela.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_cadastroDeUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [When(@"Clicar no botão registrar")]
        public void QuandoClicarNoBotaoRegistrar()
        {
            _cadastroDeUsuarioTela.ClicarNoBotaoRegistrar();
        }
        
        [When(@"Preencher os dados do formulario com uma senha sem maiusculas")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemMaiusculas(Table table)
        {
            // Arrange
            _automacaoWebTestsFixture.GerarDadosUsuario();
            var usuario = _automacaoWebTestsFixture.Usuario;
            usuario.Senha = "teste@123";

            // Act
            _cadastroDeUsuarioTela.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_cadastroDeUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [When(@"Preencher os dados do formulario com uma senha sem caractere especial")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemCaractereEspecial(Table table)
        {
            // Arrange
            _automacaoWebTestsFixture.GerarDadosUsuario();
            var usuario = _automacaoWebTestsFixture.Usuario;
            usuario.Senha = "Teste123";

            // Act
            _cadastroDeUsuarioTela.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_cadastroDeUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiuscula")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmaLetraMaiuscula()
        {
            Assert.True(_cadastroDeUsuarioTela.ValidarMensagemDeErroFormulario("Passwords must have at least one uppercase ('A'-'Z')"));
        }
        
        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmCaractereEspecial()
        {
            Assert.True(_cadastroDeUsuarioTela.ValidarMensagemDeErroFormulario("Passwords must have at least one non alphanumeric character"));
        }
    }
}