using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApp.Tests.Features;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("NerdStore.WebApp.Tests.Features.PriorityOrderer", "NerdStore.WebApp.Tests")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public UsuarioTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Realizar Cadastro com Sucesso"), TestPriority(2)]
        [Trait("Categoria", "Integração Web - Usuário")]
        public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
        {
            // Arrange
            var inicialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            inicialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await inicialResponse.Content.ReadAsStringAsync());

            _testsFixture.GerarUsuarioSenha();

            var formData = new Dictionary<string, string> { 
                { "Input.Email", _testsFixture.UsuarioEmail },
                { "Input.Password", _testsFixture.UsuarioSenha },
                { "Input.ConfirmPassword", _testsFixture.UsuarioSenha},
                { _testsFixture.AntiForgeryFieldName, antiForgeryToken}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register") {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResult = await _testsFixture.Client.SendAsync(postRequest);

            //Assert
            postResult.EnsureSuccessStatusCode();

            var responseString = await postResult.Content.ReadAsStringAsync();
            Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);

        }

        [Fact(DisplayName = "Realizar Cadastro com senha fraca")]
        [TestPriority(1)]
        [Trait("Categoria", "Integração Web - Usuário")]
        public async Task Usuario_RealizarCadastroComSenhaFraca_DeveRetornarMensagemDeErro()
        {
            // Arrange
            var inicialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            inicialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await inicialResponse.Content.ReadAsStringAsync());

            _testsFixture.GerarUsuarioSenha();
            var senhaFraca = "123456";

            var formData = new Dictionary<string, string> {
                { "Input.Email", _testsFixture.UsuarioEmail },
                { "Input.Password", senhaFraca },
                { "Input.ConfirmPassword", senhaFraca},
                { _testsFixture.AntiForgeryFieldName, antiForgeryToken}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResult = await _testsFixture.Client.SendAsync(postRequest);

            //Assert
            postResult.EnsureSuccessStatusCode();

            var responseString = await postResult.Content.ReadAsStringAsync();
            Assert.Contains("Passwords must have at least one non alphanumeric character.", responseString);

        }

        [Fact(DisplayName = "Realizar Login")]
        [TestPriority(3)]
        [Trait("Categoria", "Integração Web - Usuário")]
        public async Task Usuario_RealizarLogin_DeveExecutarComSucesso()
        {
            // Arrange
            var inicialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Login");
            inicialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await inicialResponse.Content.ReadAsStringAsync());

            //_testsFixture.GerarUsuarioSenha();

            var formData = new Dictionary<string, string> {
                { "Input.Email", _testsFixture.UsuarioEmail },
                { "Input.Password", _testsFixture.UsuarioSenha },
                { _testsFixture.AntiForgeryFieldName, antiForgeryToken}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResult = await _testsFixture.Client.SendAsync(postRequest);

            //Assert
            postResult.EnsureSuccessStatusCode();

            var responseString = await postResult.Content.ReadAsStringAsync();
            Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);

        }
    }
}
