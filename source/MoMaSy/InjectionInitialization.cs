using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Settings;
using outcold.MoMaSy.Data;
using outcold.MoMaSy.Services;
using outcold.MoMaSy.Views;
using outcold.MoMaSy.Views.Settings;
using outcold.MoMaSy.Views.Transactions;

namespace outcold.MoMaSy
{
    static class InjectionInitialization
    {
        private static IoC _ioc = IoC.Instance;

        public static void RegisterCoreObjects()
        {
            _ioc.RegisterSingleton<ISettingsStore, SettingsStore>();
        }

        public static void RegisterStorage(Storage storage)
        {
            _ioc.RegisterInstance(storage);
            _ioc.Register<StorageContext>();
        }

        public static void RegisterServices()
        {
            _ioc.RegisterSingleton<ICurrencyTypesService, CurrencyTypesService>();
            _ioc.RegisterSingleton<IAccountsService, AccountsService>();
            _ioc.RegisterSingleton<ITransactionTypesService, TransactionTypesService>();
            _ioc.RegisterSingleton<ITransactionsService, TransactionsService>();
        }

        public static void RegisterViews()
        {
            _ioc.RegisterSingleton<INotifyService, NotifyService>();

            _ioc.RegisterSingleton<MainWindow>();

            // Main Views
            _ioc.RegisterSingleton<SettingsView>();
            _ioc.RegisterSingleton<ITransactionsView, TransactionsView>();

            // Settings Views
            _ioc.RegisterSingleton<IAccountsView, AccountsView>();
            _ioc.RegisterSingleton<ICurrencyTypesView, CurrencyTypesView>();
            _ioc.RegisterSingleton<ITransactionTypesView, TransactionTypesView>();
        }
    }
}
