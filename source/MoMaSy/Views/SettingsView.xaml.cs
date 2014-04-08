using System.Windows;
using System.Windows.Controls;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Views.Settings;

namespace outcold.MoMaSy.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private IoC _ioc;

        public SettingsView(IoC ioc)
        {
            _ioc = ioc;
            InitializeComponent();
            GoToTransactionTypesView(this, null);
        }

        private void GoToAccountsView(object sender, RoutedEventArgs e)
        {
            Frame.Content = _ioc.Resolve<IAccountsView>();
        }

        private void GoToCurrencyTypesView(object sender, RoutedEventArgs e)
        {
            Frame.Content = _ioc.Resolve<ICurrencyTypesView>();
        }

        private void GoToTransactionTypesView(object sender, RoutedEventArgs e)
        {
            Frame.Content = _ioc.Resolve<ITransactionTypesView>();
        }
    }
}
