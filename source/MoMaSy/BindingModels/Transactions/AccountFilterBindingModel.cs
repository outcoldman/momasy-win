using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.BindingModels.Transactions
{
    internal class AccountFilterBindingModel : BindingModel
    {
        private IAccount _account;
        private bool _isSelected;

        public AccountFilterBindingModel(IAccount account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            _account = account;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected);
                }
            }
        }

        public string Name
        {
            get { return _account.AccountFullName; }
        }

        public IAccount Item
        {
            get
            {
                return _account;
            }
        }

        public decimal Amount
        {
            get
            {
                var service = IoC.Resolve<IAccountsService>();
                return service.GetAccountAmount(Item);
            }
        }

        public void RefreshAmount()
        {
            RaisePropertyChanged(() => Amount);
        }
    }
}
