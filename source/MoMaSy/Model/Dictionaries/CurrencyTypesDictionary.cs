using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.Model.Dictionaries
{
    internal class CurrencyTypesDictionary : DomainDictionaryBase<ICurrencyType, ICurrencyTypesService>
    {
        protected override IEnumerable<ICurrencyType> PrepareCollection(IEnumerable<ICurrencyType> collection)
        {
            return collection.Where(x => !x.IsHidden).OrderBy(x => x.CurrencyName);
        }
    }
}
