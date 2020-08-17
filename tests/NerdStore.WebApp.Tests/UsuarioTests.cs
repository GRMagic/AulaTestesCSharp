using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public UsuarioTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Realizar Cadastro com Sucesso")]
        [Trait("Categoria", "Integração Web - Usuário")]
        public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
        {
            // Arrange
            var inicialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            inicialResponse.EnsureSuccessStatusCode();

            var email = "teste@teste.com";
            var formData = new Dictionary<string, string> { 
                { "Input.Email", email },
                { "Input.Password", "Teste@123" },
                { "Input.ConfirmPassword", "Teste@123"}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register") {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResult = await _testsFixture.Client.SendAsync(postRequest);

            //Assert
            postResult.EnsureSuccessStatusCode();

            var responseString = await postResult.Content.ReadAsStringAsync();
            Assert.Contains($"Hello {email}", responseString);

        }
    }
}
