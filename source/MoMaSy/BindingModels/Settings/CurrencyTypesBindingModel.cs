using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.WebServices.Currencies;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal class CurrencyTypesBindingModel : CollectionBindingModel<ICurrencyType, CurrencyTypeBindingModel>
    {
        private RatesServicesFactory _serviceFactory = new RatesServicesFactory();

        private List<IRatesService> _services;

        public CurrencyTypesBindingModel()
        {
            _services = new List<IRatesService>(_serviceFactory.GetServices());
        }

        public List<IRatesService> Services
        {
            get { return _services; }
        }

        public IRatesService SelectedRatesService { get; set; }

        public void SetCurrencyTypes(IEnumerable<ICurrencyType> currencyTypes)
        {
            var bindingModels = currencyTypes.OrderBy(x => new Tuple<bool, string>(x.IsHidden, x.CurrencyName)).Select(x => new CurrencyTypeBindingModel(x));
            Items = new ObservableCollection<CurrencyTypeBindingModel>(bindingModels);
        }
    }
}
