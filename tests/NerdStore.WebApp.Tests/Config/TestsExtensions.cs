using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.WebApp.Tests.Config
{
    public static class TestsExtensions
    {
        // Não acho essa a melhor forma de fazer isso, mas vou seguir conforme nas aulas porque achei o parâmetro do where peculiar
        public static decimal ApenasNumeros(this string value)
        {
            return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient httpClient, string requestUri, object content)
        {
            return await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
        }
    }
}
