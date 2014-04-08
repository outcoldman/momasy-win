using System;
using System.Windows;
using System.Windows.Controls;

namespace outcold.MoMaSy.Core.Presentation
{
    public class View : UserControl, IView
    {
        public static readonly DependencyProperty IsBusyProperty =
           DependencyProperty.Register("IsBusy", typeof(bool), typeof(View), new UIPropertyMetadata(false));

        public View()
        {
            if (Style == null)
                Style = Application.Current.Resources[typeof (View)] as Style;
        }

        public bool IsBusy
        {
            get
            {
                bool result = false;
                Dispatcher.Do(() => result = (bool)GetValue(IsBusyProperty));
                return result;
            }
            set
            {
                Dispatcher.DoAsync(() => SetValue(IsBusyProperty, value));
            }
        }

        public bool IsInDesignTool
        {
            get
            {
                return DesignHelpers.DesignMode;
            }
        }

        protected dynamic PresenterDynamic { get; private set; }

        internal TPresenter CreatePresenter<TPresenter>() 
            where TPresenter : IPresenter
        {
            Type type = typeof (TPresenter);
            TPresenter presenter = (TPresenter)Activator.CreateInstance(type, this);
            PresenterDynamic = presenter;
            return presenter;
        }
    }
}
