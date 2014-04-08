using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using outcold.MoMaSy.BindingModels.Settings;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Presenters.Settings;

namespace outcold.MoMaSy.Views.Settings
{
    interface IAccountsView : IView
    {
    }

    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsView : View, IAccountsView
    {
        private AccountsViewPresenter _presenter;

        public AccountsView()
        {
            InitializeComponent();
            _presenter = base.CreatePresenter<AccountsViewPresenter>();
            KeyDown += (sender, e) =>
            {
                if (e.Handled || Keyboard.Modifiers != ModifierKeys.None)
                    return;

                if (e.Key == Key.Escape)
                    _presenter.UndoCurrentAccount();
                else if (e.Key == Key.Enter)
                    _presenter.SaveCurrentAccount();
            };
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.EditCurrentAccount();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.AddAccount();
            ListViewAccounts.ScrollIntoView(ListViewAccounts.SelectedItem);
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.DeleteAccount();
        }

        private void ListViewAccountsSelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != ListViewAccounts)
                return;

            Debug.Assert(e.AddedItems.Count == 0 || e.AddedItems.Count == 1, "e.AddedItems.Count == 0 || e.AddedItems.Count == 1");
            Debug.Assert(e.RemovedItems.Count == 0 || e.RemovedItems.Count == 1, "e.RemovedItems.Count == 0 || e.RemovedItems.Count == 1");

            Debug.Assert(e.RemovedItems.Count == 0 || e.RemovedItems[0] is AccountBindingModel
                    , "e.AddedItems.Count == 0 || e.AddedItems[0] is AccountBindingModel");

            var oldSelectedAccount = e.RemovedItems.Cast<AccountBindingModel>().FirstOrDefault();
            if (oldSelectedAccount != null && oldSelectedAccount.HasChanges)
                _presenter.SaveAccount(oldSelectedAccount);
        }
    }
}
