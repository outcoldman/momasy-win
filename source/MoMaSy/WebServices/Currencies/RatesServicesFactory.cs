using System.Collections.Generic;

namespace outcold.MoMaSy.WebServices.Currencies
{
    internal class RatesServicesFactory
    {
        private static IRatesService _eurBasedService = new EuropaRatesService();

        public IEnumerable<IRatesService> GetServices()
        {
            yield return _eurBasedService;
        }
    }
}
