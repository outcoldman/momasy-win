using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using outcold.MoMaSy.BindingModels.Transactions;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Services;
using outcold.MoMaSy.Views.Transactions;

namespace outcold.MoMaSy.Presenters.Transactions
{
    internal class TransactionsViewPresenter : Presenter<ITransactionsView, TransactionsBindingModel>
    {
        private IAccountsService _accountsService;
        private ITransactionsService _transactionsService;

        public TransactionsViewPresenter(ITransactionsView view)
            : base(view)
        {
            BindingModel.FilterBindingModel.StartDate = DateTime.Today.AddDays(-10);
            BindingModel.FilterBindingModel.FinishDate = DateTime.Today;

            if (!view.IsInDesignTool)
            {
                _accountsService = IoC.Resolve<IAccountsService>();
                _transactionsService = IoC.Resolve<ITransactionsService>();
                _accountsService.CollectionChanged += LoadFilter;
                LoadFilter();
            }
        }

        public void AddTransaction()
        {
            TransactionItemBindingModel bm = null;
            DoBackground(() =>
                {
                    bm = new TransactionItemBindingModel(_transactionsService.GetNewTransaction());
                },
                () =>
                {
                    Debug.Assert(bm != null, "bm != null");
                    BindingModel.TransactionBindingModel = bm;
                    BindingModel.FilterBindingModel.RefreshAmounts();
                });
        }

        public void EditTransaction()
        {
            Debug.Assert(BindingModel.ListBindingModel.SelectedTransaction != null, "BindingModel.ListBindingModel.SelectedTransaction != null");
            BindingModel.TransactionBindingModel = BindingModel.ListBindingModel.SelectedTransaction;
        }

        public void DeleteTransaction()
        {
            Debug.Assert(BindingModel.ListBindingModel.SelectedTransaction != null, "BindingModel.ListBindingModel.SelectedTransaction != null");
            DoBackground(() =>
                {
                    var transaction = BindingModel.ListBindingModel.SelectedTransaction.GetItem(dirty: false);
                    _transactionsService.Delete(transaction);
                },
                () =>
                {
                    LoadTransactions();
                    BindingModel.FilterBindingModel.RefreshAmounts();
                });
        }
        
        public void SaveTransaction()
        {
            Debug.Assert(BindingModel.TransactionBindingModel != null, "BindingModel.TransactionBindingModel != null");

            TransactionItemBindingModel bm = BindingModel.TransactionBindingModel;

            DoBackground(() =>
                {
                    bm.ValidateAll();
                    if (bm.IsValid)
                    {
                        if (bm.HasChanges)
                        {
                            ITransaction transaction = bm.GetItem();
                            _transactionsService.Save(transaction);
                        }
                    }
                    else
                    {
                        string error = bm.Error;
                        Debug.Assert(!string.IsNullOrWhiteSpace(error), "!string.IsNullOrWhiteSpace(error)");
                        var notifyService = IoC.Resolve<INotifyService>();
                        notifyService.ShowInformation(error);
                    }
                },
                () =>
                {
                    if (bm.IsValid)
                        CancelEditTransaction();

                    LoadTransactions();
                    BindingModel.FilterBindingModel.RefreshAmounts();
                });
        }

        public void CancelEditTransaction()
        {
            Debug.Assert(BindingModel.TransactionBindingModel != null, "BindingModel.TransactionBindingModel != null");
            BindingModel.TransactionBindingModel = null;
        }

        private void LoadFilter()
        {
            IEnumerable<IAccount> accounts = null;
            DoBackground(() =>
            {
                BindingModel.FilterBindingModel.FilterChanged -= LoadTransactions;
                accounts = _accountsService.GetAll().Where(x => !x.IsHidden).OrderBy(x => x.AccountName);
            },
            () =>
            {
                Debug.Assert(accounts != null);
                BindingModel.FilterBindingModel.SetAccounts(accounts);
                BindingModel.FilterBindingModel.FilterChanged += LoadTransactions;
                LoadTransactions();
            });
        }

        private void LoadTransactions()
        {
            IEnumerable<ITransaction> transactions = null;
            DoBackground(() =>
                {
                    var filter = BindingModel.FilterBindingModel.GetFilter();
                    transactions = _transactionsService.GetAll(filter);
                },
                () =>
                {
                    Debug.Assert(transactions != null);
                    BindingModel.ListBindingModel.SetTransactions(transactions);
                });
            
        }
    }
}
