using System.Collections.Generic;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.WebServices.Currencies
{
    internal interface IRatesService
    {
        string ServiceName { get; }
        ICurrencyType GetBaseCurrency();
        IEnumerable<IRate> GetRates();
    }
}
