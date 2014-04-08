using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data;
using outcold.MoMaSy.Views;

namespace outcold.MoMaSy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _appMutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InjectionInitialization.RegisterCoreObjects();

            Storage storage = new Storage(IoC.Instance);

            bool createdNew;
            _appMutex = new Mutex(false, GetSigletonApplicationId(storage.ConnectionString), out createdNew);
            
            if (createdNew)
            {
                storage.Init();

                InjectionInitialization.RegisterStorage(storage);
                InjectionInitialization.RegisterServices();
                InjectionInitialization.RegisterViews();

                MainWindow = IoC.Instance.Resolve<MainWindow>();
                MainWindow.Show();
            }
            else
            {
                // TODO: Need to register COM and active it on loading when mutex is not created
                Shutdown(1);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _appMutex.Close();
        }

        /// <summary>
        /// Create hash code (in fact this is not hash code) based on <param name="str" />. 
        /// We are using path to data file (connection string).
        /// </summary>
        private string GetSigletonApplicationId(string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            string result = Convert.ToBase64String(str.ToCharArray().Select(x => (byte)((int)x % byte.MaxValue)).ToArray());

            const int mutexMaxNameLength = 260;

            if (result.Length >= mutexMaxNameLength)
                result = result.Substring(0, mutexMaxNameLength);
            
            return result;
        }
    }
}
