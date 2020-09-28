using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.BDD.Tests.Config
{
    public class PageObjectModel
    {
        protected readonly SeleniumHelper Helper;

        public PageObjectModel(SeleniumHelper helper)
        {
            Helper = helper;
        }

        public string ObterUrl() => Helper.ObterUrl();

    }
}
