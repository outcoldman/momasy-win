using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using outcold.MoMaSy.BindingModels.Settings;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Presenters.Settings;

namespace outcold.MoMaSy.Views.Settings
{
    interface ICurrencyTypesView : IView
    {
        void ShowCurrencyType(CurrencyTypeBindingModel bindingModel);
    }

    /// <summary>
    /// Interaction logic for CurrencyTypesView.xaml
    /// </summary>
    public partial class CurrencyTypesView : View, ICurrencyTypesView
    {
        private CurrencyTypesViewPresenter _presenter;

        public CurrencyTypesView()
        {
            InitializeComponent();
            _presenter = CreatePresenter<CurrencyTypesViewPresenter>();
            KeyDown += (sender, e) =>
            {
                if (e.Handled || Keyboard.Modifiers != ModifierKeys.None)
                    return;

                if (e.Key == Key.Escape)
                    _presenter.UndoCurrentCurrencyType();
                else if (e.Key == Key.Enter)
                    _presenter.SaveCurrentCurrencyType();
            };
        }

        private void UpdateCurrencyTypesClick(object sender, RoutedEventArgs e)
        {
            _presenter.UpdateCurrencyTypes();
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.EditCurrentCurrencyType();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.AddCurrencyType();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.DeleteCurrencyType();
        }

        private void ListViewCurrenciesSelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != ListViewCurrencies)
                return;

            Debug.Assert(e.AddedItems.Count == 0 || e.AddedItems.Count == 1, "e.AddedItems.Count == 0 || e.AddedItems.Count == 1");
            Debug.Assert(e.RemovedItems.Count == 0 || e.RemovedItems.Count == 1, "e.RemovedItems.Count == 0 || e.RemovedItems.Count == 1");

            Debug.Assert(e.RemovedItems.Count == 0 || e.RemovedItems[0] is CurrencyTypeBindingModel
                    , "e.AddedItems.Count == 0 || e.AddedItems[0] is CurrencyTypeBindingModel");

            var oldSelectedType = e.RemovedItems.Cast<CurrencyTypeBindingModel>().FirstOrDefault();
            if (oldSelectedType != null && oldSelectedType.HasChanges)
                _presenter.SaveCurrencyType(oldSelectedType);
        }

        void ICurrencyTypesView.ShowCurrencyType(CurrencyTypeBindingModel bindingModel)
        {
            ListViewCurrencies.ScrollIntoView(bindingModel);
        }
    }
}
