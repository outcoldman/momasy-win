using System;
using System.Diagnostics;
using outcold.MoMaSy.BindingModels.Settings;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Services;
using outcold.MoMaSy.Views.Settings;

namespace outcold.MoMaSy.Presenters.Settings
{
    internal class AccountsViewPresenter : Presenter<IAccountsView, AccountsBindingModel>
    {
        private IAccountsService _accountsService;

        public AccountsViewPresenter(IAccountsView view)
            : base(view)
        {
            if (!view.IsInDesignTool)
            {
                _accountsService = IoC.Resolve<IAccountsService>();
                LoadAccounts();
            }
        }

        private void LoadAccounts()
        {
            DoBackground(() =>
            {
                var accounts = _accountsService.GetAll();
                BindingModel.SetAccounts(accounts);
            });
        }

        internal void EditCurrentAccount()
        {
            DoBackground(() =>
            {
                AccountBindingModel bm = BindingModel.SelectedBindingModel;
                Debug.Assert(bm != null, "bm != null");

                bm.IsEditMode = true;
            });
        }

        internal void UndoCurrentAccount()
        {
            AccountBindingModel bm = BindingModel.SelectedBindingModel;
            Debug.Assert(bm != null, "bm != null");
            UndoAccount(bm);
        }

        internal void UndoAccount(AccountBindingModel accountBindingModel)
        {
            if (accountBindingModel == null)
                throw new ArgumentNullException("accountBindingModel");

            DoBackground(() =>
            {
                IAccount account = accountBindingModel.GetItem();
                Debug.Assert(account != null, "account != null");
                accountBindingModel.SetItem(_accountsService.Get(account));
                accountBindingModel.IsEditMode = false;
            });
        }

        internal void SaveCurrentAccount()
        {
            AccountBindingModel bm = BindingModel.SelectedBindingModel;
            Debug.Assert(bm != null, "bm != null");
            SaveAccount(bm);
        }

        internal void SaveAccount(AccountBindingModel accountBindingModel)
        {
            if (accountBindingModel == null)
                throw new ArgumentNullException("accountBindingModel");

            DoBackground(() =>
            {
                accountBindingModel.ValidateAll();
                if (accountBindingModel.IsValid)
                {
                    if (accountBindingModel.HasChanges)
                    {
                        IAccount account = accountBindingModel.GetItem();
                        _accountsService.Save(account);
                        accountBindingModel.SetItem(account);
                    }

                    accountBindingModel.IsEditMode = false;
                }
            });
        }

        internal void AddAccount()
        {
            AccountBindingModel bm = null;
            DoBackground(() =>
            {
                string errorMessage;
                if (!_accountsService.CanCreateNew(out errorMessage))
                {
                    Debug.Assert(!string.IsNullOrWhiteSpace(errorMessage));
                    NotifyService.ShowError(errorMessage);
                    return false;
                }

                var account = _accountsService.CreateNew();
                Debug.Assert(account != null, "account != null");
                bm = new AccountBindingModel(account);
                return true;
            },
            () =>
            {
                Debug.Assert(bm != null, "bm != null");
                BindingModel.Items.Add(bm);
                BindingModel.SelectedBindingModel = bm;
                bm.IsEditMode = true;
            });
        }

        internal void DeleteAccount()
        {
            AccountBindingModel bm = BindingModel.SelectedBindingModel;
            Debug.Assert(bm != null, "bm != null");
            
            DoBackground(() =>
            {
                IAccount account = bm.GetItem();

                Debug.Assert(account != null, "account != null");
                
                string errorMessage;
                if (!_accountsService.CanBeDeleted(account, out errorMessage))
                {
                    Debug.Assert(!string.IsNullOrWhiteSpace(errorMessage));
                    NotifyService.ShowError(errorMessage);
                    return false;
                }

                _accountsService.Delete(account);
                return true;
            },
            () =>
            {
                Debug.Assert(bm != null, "bm != null");
                int index = BindingModel.Items.IndexOf(bm);
                BindingModel.Items.Remove(bm);
                if (BindingModel.Items.Count > 0)
                {
                    Debug.Assert(BindingModel.Items.Count >= index, "BindingModel.Items.Count <= index");
                    if (BindingModel.Items.Count <= index)
                        index--;
                    BindingModel.SelectedBindingModel = BindingModel.Items[index];
                }
            });
        }
    }
}
