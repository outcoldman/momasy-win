using System;
using System.Diagnostics;
using System.Linq;
using outcold.MoMaSy.BindingModels.Settings;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Services;
using outcold.MoMaSy.Views.Settings;
using outcold.MoMaSy.WebServices.Currencies;

namespace outcold.MoMaSy.Presenters.Settings
{
    internal class CurrencyTypesViewPresenter : Presenter<ICurrencyTypesView, CurrencyTypesBindingModel>
    {
        private ICurrencyTypesService _currencyTypesService;

        public CurrencyTypesViewPresenter(ICurrencyTypesView view)
            : base(view)
        {
            if (!view.IsInDesignTool)
            {
                _currencyTypesService = IoC.Resolve<ICurrencyTypesService>();
                LoadCurrencyTypes();
            }
        }

        private void LoadCurrencyTypes()
        {
            DoBackground(() =>
            {
                var currencyTypes = _currencyTypesService.GetAll();
                BindingModel.SetCurrencyTypes(currencyTypes);
            });
        }

        internal void UpdateCurrencyTypes()
        {
            IRatesService service = BindingModel.SelectedRatesService;
            Debug.Assert(service != null, "service != null");

            DoBackground(() =>
            {
                var types = service.GetRates().Union(new[] { service.GetBaseCurrency() });
                Debug.Assert(types != null, "types != null");

                var dbTypes = _currencyTypesService.GetAll();

                foreach (ICurrencyType type in types)
                {
                    bool contains = dbTypes.Any(x => string.Equals(x.CurrencyName, type.CurrencyName, StringComparison.CurrentCultureIgnoreCase));
                    if (!contains)
                    {
                        CurrencyTypeModel currencyType = new CurrencyTypeModel(new CurrencyType() 
                        {
                            CurrencyName = type.CurrencyName,
                            IsHidden = true
                        });
                        _currencyTypesService.Add(currencyType);
                    }
                }
            }, () => LoadCurrencyTypes());
        }

        internal void EditCurrentCurrencyType()
        {
            CurrencyTypeBindingModel bm = BindingModel.SelectedBindingModel;
            Debug.Assert(bm != null, "bm != null");
            bm.IsEditMode = true;
        }

        internal void UndoCurrentCurrencyType()
        {
            CurrencyTypeBindingModel bm = BindingModel.SelectedBindingModel;
            Debug.Assert(bm != null, "bm != null");

            UndoCurrencyType(bm);
        }

        internal void UndoCurrencyType(CurrencyTypeBindingModel bindigModel)
        {
            if (bindigModel == null)
                throw new ArgumentNullException("bindigModel");

            DoBackground(() =>
            {
                ICurrencyType currencyType = bindigModel.GetItem();
                Debug.Assert(currencyType != null, "currencyType != null");
                bindigModel.SetItem(_currencyTypesService.Get(currencyType));
                bindigModel.IsEditMode = false;
            });
        }

        internal void SaveCurrentCurrencyType()
        {
            CurrencyTypeBindingModel bm = BindingModel.SelectedBindingModel;
            Debug.Assert(bm != null, "bm != null");

            SaveCurrencyType(bm);
        }

        internal void SaveCurrencyType(CurrencyTypeBindingModel bindingModel)
        {
            if (bindingModel == null)
                throw new ArgumentNullException("bindigModel");

            DoBackground(() =>
            {
                bindingModel.ValidateAll();
                if (bindingModel.IsValid)
                {
                    if (bindingModel.HasChanges)
                    {
                        ICurrencyType currencyType = bindingModel.GetItem();
                        _currencyTypesService.Save(currencyType);
                        bindingModel.SetItem(currencyType);
                    }

                    bindingModel.IsEditMode = false;
                }
            });
        }

        internal void AddCurrencyType()
        {
            CurrencyTypeBindingModel bm = null;
            DoBackground(() =>
            {
                var currencyType = _currencyTypesService.CreateNew();
                Debug.Assert(currencyType != null, "currencyType != null");
                bm = new CurrencyTypeBindingModel(currencyType);
            },
            () =>
            {
                Debug.Assert(bm != null, "bm != null");
                BindingModel.Items.Add(bm);
                BindingModel.SelectedBindingModel = bm;
                bm.IsEditMode = true;
                View.ShowCurrencyType(bm);
            });
        }

        internal void DeleteCurrencyType()
        {
            CurrencyTypeBindingModel bm = BindingModel.SelectedBindingModel;
            Debug.Assert(bm != null, "bm != null");

            DoBackground(() =>
            {
                ICurrencyType currencyType = bm.GetItem();

                Debug.Assert(currencyType != null, "currencyType != null");
                string errorMessage;
                if (!_currencyTypesService.CanBeDeleted(currencyType, out errorMessage))
                {
                    Debug.Assert(!string.IsNullOrWhiteSpace(errorMessage));
                    NotifyService.ShowError(errorMessage);
                    return false;
                }

                _currencyTypesService.Delete(currencyType);
                return true;
            },
            () =>
            {
                int index = BindingModel.Items.IndexOf(bm);
                BindingModel.Items.Remove(bm);
                if (BindingModel.Items.Count > 0)
                {
                    Debug.Assert(BindingModel.Items.Count >= index, "BindingModel.Items.Count <= index");
                    if (BindingModel.Items.Count <= index)
                        index--;
                    BindingModel.SelectedBindingModel = BindingModel.Items[index];
                    View.ShowCurrencyType(BindingModel.SelectedBindingModel);
                }
            });
        }
    }
}
