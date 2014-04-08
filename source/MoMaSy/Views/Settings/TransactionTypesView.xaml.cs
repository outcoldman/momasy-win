using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using outcold.MoMaSy.BindingModels.Settings;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Presenters.Settings;

namespace outcold.MoMaSy.Views.Settings
{
    internal interface ITransactionTypesView : IView
    {
    }

    /// <summary>
    /// Interaction logic for TransactionTypesView.xaml
    /// </summary>
    public partial class TransactionTypesView : View, ITransactionTypesView
    {
        private TransactionTypesViewPresenter _presenter;

        public TransactionTypesView()
        {
            InitializeComponent();
            _presenter = CreatePresenter<TransactionTypesViewPresenter>();
            KeyDown += (sender, e) =>
            {
                if (e.Handled || Keyboard.Modifiers != ModifierKeys.None)
                    return;

                if (e.Key == Key.Escape)
                    _presenter.UndoCurrentTransactionType();
                else if (e.Key == Key.Enter)
                    _presenter.SaveCurrentTransactionType();
            };
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.AddTransactionType();
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.EditCurrentTransactionType();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.DeleteTransactionType();
        }

        private void TypesTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Debug.Assert(e.OldValue == null || e.OldValue is TransactionTypeBindingModelBase, "e.OldValue == null || e.OldValue is TransactionTypeBindingModelBase");

            var oldSelectedTransactionType = e.OldValue as TransactionTypeBindingModelBase;
            if (oldSelectedTransactionType != null
                && !oldSelectedTransactionType.IsSystem
                && oldSelectedTransactionType.HasChanges)
                _presenter.SaveTransactionType(oldSelectedTransactionType);
        }
    }
}
