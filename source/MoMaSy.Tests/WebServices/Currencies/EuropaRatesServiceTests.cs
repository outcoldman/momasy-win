using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using outcold.MoMaSy.WebServices.Currencies;

namespace MoMaSy.Tests.WebServices.Currencies
{
    [TestClass]
    public class EuropaRatesServiceTests
    {
        [TestMethod]
        public void GetRates_ShouldReturnRates()
        {
            EuropaRatesService service = new EuropaRatesService();
            var rates = service.GetRates();
            Assert.IsNotNull(rates);
            Assert.IsTrue(rates.Count() > 0);
        }
    }
}
