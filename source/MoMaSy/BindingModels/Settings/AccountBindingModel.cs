using System;
using System.Collections.Generic;
using System.Linq;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal class AccountBindingModel : CollectionItemBindingModel<AccountBindingModel, IAccount>
    {
        private ICurrencyTypesService _currencyTypesService;
        private string _accountName;
        private ICurrencyType _currencyType;
        private bool _isHidden;

        public AccountBindingModel(IAccount account)
            :base (account)
        {
            _currencyTypesService = IoC.Resolve<ICurrencyTypesService>(); 

            AddValidationFor(() => AccountName)
                .When(x => string.IsNullOrWhiteSpace(x.AccountName) || x.AccountName.Length > 30)
                .Show("Account Name is required (Max length 30 symbols)");

            AddValidationFor(() => CurrencyType)
                .When(x => x.CurrencyType == null)
                .Show("Select Currency Type");
        }

        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                if (value != _accountName)
                {
                    _accountName = value;
                    RaisePropertyChanged(() => AccountName);
                    RaisePropertyChanged(() => AccountFullName);
                }
            }
        }

        public ICurrencyType CurrencyType
        {
            get { return _currencyType; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (_currencyType == null || !_currencyType.Equals(value))
                {
                    _currencyType = value;
                    RaisePropertyChanged(() => CurrencyType);
                    RaisePropertyChanged(() => AccountFullName);
                }
            }
        }

        public string AccountFullName
        {
            get 
            {
                return string.Format(StringResources.Account_FullNameFormat, AccountName, CurrencyType.CurrencyName);; 
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
                    RaisePropertyChanged(() => HasChanges);
                }
            }
        }

        public override bool HasChanges
        {
            get
            {
                return Item.IsHidden != IsHidden ||
                    !Item.CurrencyType.Equals(CurrencyType) ||
                    !string.Equals(Item.AccountName, AccountName, StringComparison.CurrentCulture);
            }
        }

        protected override void SaveData()
        {
            Item.AccountName = AccountName;
            Item.IsHidden = IsHidden;
            Item.CurrencyType = CurrencyType;
        }

        protected override void LoadData()
        {
            AccountName = Item.AccountName;
            IsHidden = Item.IsHidden;
            CurrencyType = Item.CurrencyType;
        }
    }
}
