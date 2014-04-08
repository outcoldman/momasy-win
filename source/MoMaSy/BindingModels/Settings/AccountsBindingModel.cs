using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal class AccountsBindingModel : CollectionBindingModel<IAccount, AccountBindingModel>
    {
        public AccountsBindingModel()
        {
        }
               
        internal void SetAccounts(IEnumerable<IAccount> accounts)
        {
            var bindingModels = accounts.Select(x => new AccountBindingModel(x));
            Items = new ObservableCollection<AccountBindingModel>(bindingModels);
        }
    }
}
