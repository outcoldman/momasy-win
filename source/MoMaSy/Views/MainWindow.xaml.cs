using System;
using System.Windows;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Presenters;
using outcold.MoMaSy.Services;
using outcold.MoMaSy.Views.Transactions;

namespace outcold.MoMaSy.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        private IoC _ioc;
        private MainWindowPresenter _presenter;

        public MainWindow(IoC ioc)
        {
            _ioc = ioc;
            _presenter = new MainWindowPresenter(this);
            InitializeComponent();

            GoToTransactionView(silent: true);
        }

        private void GoToTransactionsView(object sender, EventArgs e)
        {
            GoToTransactionView(silent:false);
        }

        private void GoToSettingsView(object sender, EventArgs e)
        {
            MainFrame.Content = _ioc.Resolve<SettingsView>();
        }

        private void GoToTransactionView(bool silent)
        {
            if (_presenter.CheckCanAddTransaction(silent))
            {
                MainFrame.Content = _ioc.Resolve<ITransactionsView>();
            }
            else
            {
                GoToSettingsView(this, RoutedEventArgs.Empty);
            }
        }

        public bool IsBusy
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInDesignTool
        {
            get { return DesignHelpers.DesignMode; }
        }
    }
}
