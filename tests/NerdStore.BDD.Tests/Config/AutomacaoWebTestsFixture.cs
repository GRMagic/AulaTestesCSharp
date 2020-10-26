using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NerdStore.BDD.Tests.Config
{
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    [TechTalk.SpecFlow.Scope()]
    public class AutomacaoWebFixtureCollection : ICollectionFixture<AutomacaoWebTestsFixture> { }

    public class AutomacaoWebTestsFixture
    {
        public SeleniumHelper BrowserHelper;
        public readonly ConfigurationHelper Configuration;
        public Usuario.Usuario Usuario;

        public AutomacaoWebTestsFixture()
        {
            Configuration = new ConfigurationHelper();
            BrowserHelper = new SeleniumHelper(Configuration);// Browser.Chrome, Configuration, false);
            Usuario = new Usuario.Usuario();
        }

        public void GerarDadosUsuario()
        {
            var faker = new Faker("pt_BR");
            Usuario.Email = faker.Internet.Email().ToLower();
            Usuario.Senha = faker.Internet.Password(8, false, "", "@1Ab_");
        }
    }
}
