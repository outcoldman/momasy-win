using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.Presenters
{
    internal class MainWindowPresenter : Presenter<IView>
    {
        public MainWindowPresenter(IView view)
            : base(view)
        {

        }

        public bool CheckCanAddTransaction(bool silent)
        {
            string errMessage;
            var service = IoC.Resolve<ITransactionsService>();
            bool result = service.CanAddTransaction(out errMessage);

            if (!silent && !result)
            {
                Debug.Assert(!string.IsNullOrEmpty(errMessage), "string.IsNullOrEmpty(errMessage)");
                IoC.Resolve<INotifyService>().ShowError(errMessage);
            }

            return result;
        }
    }
}
