using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Data.DataModel;

namespace outcold.MoMaSy.Model
{
    internal interface ICurrencyType
    {
        string CurrencyName { get; set; }
        bool IsHidden { get; set; }
    }

    internal class CurrencyTypeModel : EntityModel<CurrencyType>, ICurrencyType
    {
        public CurrencyTypeModel(CurrencyType currencyType)
            : base(currencyType)
        {

        }

        public string CurrencyName
        {
            get { return DataItem.CurrencyName; }
            set { DataItem.CurrencyName = value; }
        }

        public bool IsHidden
        {
            get { return DataItem.IsHidden; }
            set { DataItem.IsHidden = value; }
        }
    }
}
