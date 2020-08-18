using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.MVC;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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


        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
