using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Resources.Strings;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.Model
{
    internal interface IAccount
    {
        string AccountName { get; set; }
        bool IsHidden { get; set; }
        string AccountFullName { get; }

        ICurrencyType CurrencyType { get; set; }
    }

    internal class AccountModel : EntityModel<Account>, IAccount
    {
        private ICurrencyTypesService _currencyTypesService; 

        public AccountModel(IoC ioc, Account account)
            : base(account)
        {
            _currencyTypesService = ioc.Resolve<ICurrencyTypesService>();
        }

        public string AccountName
        {
            get { return DataItem.AccountName; }
            set { DataItem.AccountName = value; }
        }

        public bool IsHidden
        {
            get { return DataItem.IsHidden; }
            set { DataItem.IsHidden = value; }
        }

        public string AccountFullName
        {
            get
            {
                return string.Format(StringResources.Account_FullNameFormat, AccountName, CurrencyType.CurrencyName);
            }
        }

        public ICurrencyType CurrencyType
        {
            get
            {
                return _currencyTypesService.Get(DataItem.CurrencyTypeId);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Currency Type cannot be null");

                if (!(value is CurrencyTypeModel))
                    throw new ArgumentException("value", "Couldn't get information about Entity. Should be CurrencyTypeModel.");

                DataItem.CurrencyTypeId = ((CurrencyTypeModel)value).Id;
            }
        }
    }
}
