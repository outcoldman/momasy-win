using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.Model.Dictionaries
{
    internal class AccountsDictionary : DomainDictionaryBase<IAccount, IAccountsService>
    {
        protected override IEnumerable<IAccount> PrepareCollection(IEnumerable<IAccount> collection)
        {
            return collection.Where(x => !x.IsHidden).OrderBy(x => x.AccountFullName);
        }
    }
}
