using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.MVC;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests.Config
{

    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupWebTests>>{ }


    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>> { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly LojaAppFactory<TStartup> Factory;
        public HttpClient Client;

        public string AntiForgeryFieldName = "__RequestVerificationToken";
        public string UsuarioEmail;
        public string UsuarioSenha;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true, // O default já é true
                MaxAutomaticRedirections = 7, // O default já é 7
                HandleCookies = true, // O default já é true
                BaseAddress = new Uri("http://localhost")// O default já é http://localhost
            };
            Factory = new LojaAppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public string ObterAntiForgeryToken(string htmlBody)
        {
            var match = Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");
            if (match.Success)
            {
                return match.Groups[1].Captures[0].Value;
            }
            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' não encontrado no HTML", nameof(htmlBody));
        }

        public void GerarUsuarioSenha()
        {
            var faker = new Faker("pt_BR");
            UsuarioEmail = faker.Internet.Email();
            UsuarioSenha = faker.Internet.Password(8, false, "", "@1Ab_");
        }

        public async Task RealizarLoginWeb()
        {
            var initialResponse = await Client.GetAsync("/Identity/Account/Login");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var formData = new Dictionary<string, string>
            {
                {AntiForgeryFieldName, antiForgeryToken},
                {"Input.Email", "qual_e_a_treta@hotmail.com"},
                {"Input.Password", "Teste@123"}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            await Client.SendAsync(postRequest);
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
