using System;
using System.Text.RegularExpressions;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal class CurrencyTypeBindingModel : CollectionItemBindingModel<CurrencyTypeBindingModel, ICurrencyType>
    {
        private string _currencyName;
        private bool _isHidden;

        public CurrencyTypeBindingModel(ICurrencyType currencyType)
            : base(currencyType)
        {
            AddValidationFor(() => CurrencyName)
                .When((x) => string.IsNullOrWhiteSpace(CurrencyName) || !Regex.IsMatch(CurrencyName, @"[A-Z]{3,3}", RegexOptions.IgnoreCase))
                    .Show("Currency Name can't be null and should contains 3 letters.");
        }

        public string CurrencyName
        {
            get { return _currencyName; }
            set 
            {
                if (!string.Equals(_currencyName, value, StringComparison.CurrentCulture))
                {
                    _currencyName = value;
                    RaisePropertyChanged(() => CurrencyName);
                }
            }
        }

        public bool IsHidden
        {
            get { return _isHidden; }
            set
            {
                if (_isHidden != value)
                {
                    _isHidden = value;
                    RaisePropertyChanged(() => IsHidden);
                }
            }
        }

        public override bool HasChanges
        {
            get
            {
                return Item.IsHidden != IsHidden ||
                    !string.Equals(Item.CurrencyName, CurrencyName, StringComparison.CurrentCulture);
            }
        }

        protected override void LoadData()
        {
            CurrencyName = Item.CurrencyName;
            IsHidden = Item.IsHidden;
        }

        protected override void SaveData()
        {
            Item.CurrencyName = CurrencyName;
            Item.IsHidden = IsHidden;
        }
    }
}
