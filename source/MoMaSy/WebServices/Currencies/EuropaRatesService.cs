using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.WebServices.Currencies
{
    /// <summary>
    /// Rates service based on http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml.
    /// All rates relative to EUR
    /// </summary>
    internal class EuropaRatesService : IRatesService
    {
        private NumberFormatInfo _numberFormatInfo = new NumberFormatInfo()
            {
                CurrencyDecimalSeparator = "."
            };

        public string ServiceName
        {
            get
            {
                return "Euro-based web service";
            }
        }

        public ICurrencyType GetBaseCurrency()
        {
            return new RateModel() { CurrencyName = "EUR" };
        }

        public IEnumerable<IRate> GetRates()
        {
            XElement element = XElement.Load("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");
            XNode n = element.FirstNode;
            return element.Descendants(XName.Get("Cube", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref"))
                    .Where(x => !x.HasElements).Select(x => new RateModel()
                                                    {
                                                        CurrencyName = x.Attribute("currency").Value,
                                                        CurrencyRate = decimal.Parse(x.Attribute("rate").Value, NumberStyles.Currency, _numberFormatInfo)
                                                    }).AsEnumerable();
        }
    }
}
