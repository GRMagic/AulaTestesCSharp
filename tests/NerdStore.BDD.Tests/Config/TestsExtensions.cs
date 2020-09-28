using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdStore.BDD.Tests.Config
{
    public static class TestsExtensions
    {
        public static int ApenasNumeros(this string value) => Convert.ToInt32(new string(value.Where(char.IsDigit).ToArray()));
    }
}
