using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.BindingModels.Transactions
{
    internal class TransactionsFilterBindingModel : BindingModel
    {
        private DateTime _startDate;
        private DateTime _finishDate;
        private bool _fShowHiddens;
        private ObservableCollection<AccountFilterBindingModel> _accounts = new ObservableCollection<AccountFilterBindingModel>();

        public event Action FilterChanged;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    RaisePropertyChanged(() => StartDate);
                    RaiseFilterChanged();
                }
            }
        }

        public DateTime FinishDate
        {
            get { return _finishDate; }
            set
            {
                if (_finishDate != value)
                {
                    _finishDate = value;
                    RaisePropertyChanged(() => FinishDate);
                    RaiseFilterChanged();
                }
            }
        }

        public bool ShowHiddens
        {
            get { return _fShowHiddens; }
            set
            {
                if (_fShowHiddens != value)
                {
                    _fShowHiddens = value;
                    RaisePropertyChanged(() => ShowHiddens);
                    RaiseFilterChanged();
                }
            }
        }

        public ObservableCollection<AccountFilterBindingModel> Accounts
        {
            get { return _accounts; }
        }

        public void RefreshAmounts()
        {
            foreach (var account in _accounts)
            {
                account.RefreshAmount();
            }
        }

        public void SetAccounts(IEnumerable<IAccount> accounts)
        {
            if (accounts == null)
                throw new ArgumentNullException("accounts");

            foreach (var bm in _accounts)
            {
                bm.PropertyChanged -= OnAccountSelected;
            }

            _accounts.Clear();
            foreach (var account in accounts)
            {
                var bm = new AccountFilterBindingModel(account);
                bm.PropertyChanged += OnAccountSelected;
                // TODO: Need to store last choose
                bm.IsSelected = true;
                _accounts.Add(bm);
            }
        }

        public TransactionsFilter GetFilter()
        {
            TransactionsFilter filter = new TransactionsFilter();
            filter.StartDate = StartDate;
            filter.FinishDate = FinishDate;
            filter.ShowHidden = ShowHiddens;
            filter.Accounts = _accounts.Where(x => x.IsSelected).Select(x => x.Item);
            return filter;
        }

        private void OnAccountSelected(object sender, PropertyChangedEventArgs args)
        {
            if (string.Equals(args.PropertyName, "IsSelected"))
            {
                RaiseFilterChanged();
            }
        }

        private void RaiseFilterChanged()
        {
            var eventHandler = FilterChanged;
            if (eventHandler != null)
                eventHandler();
        }
    }
}
