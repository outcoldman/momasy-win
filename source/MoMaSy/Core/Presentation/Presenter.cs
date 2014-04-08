using System;
using System.Threading.Tasks;
namespace outcold.MoMaSy.Core.Presentation
{
    internal abstract class Presenter<TView, TBindingModel> : Presenter<TView>
        where TView : IView
        where TBindingModel : BindingModel, new()
    {
        public TBindingModel BindingModel { get; private set; }

        protected Presenter(TView view)
            : base(view)
        {
            BindingModel = new TBindingModel();
            view.DataContext = BindingModel;
        }
    }

    internal abstract class Presenter<TView> : IPresenter
        where TView : IView
    {
        private IoC _ioc;
        private INotifyService _notifyService;

        protected Presenter(TView view)
        {
            View = view;
            _ioc = IoC.Instance;
            _notifyService = _ioc.Resolve<INotifyService>();
        }

        public TView View { get; private set; }

        protected IoC IoC
        {
            get { return _ioc; }
        }

        protected INotifyService NotifyService
        {
            get { return _notifyService; }
        }

        public virtual void Initialize() { }

        protected virtual bool DetectDesignMode() { return DesignHelpers.DesignMode; }

        protected void DoBackground(Action action)
        {
            DoBackground(action, null);
        }

        protected void DoBackground(Action action, Action onComplete)
        {
            DoBackground(() => { action(); return true; }, onComplete);
        }

        protected void DoBackground(Func<bool> action, Action onSuccessComplete)
        {
            View.IsBusy = true;
            Task.Factory.StartNew(action)
                .ContinueWith((task) =>
                {
                    if (task.Result)
                    {
                        View.Dispatcher.Do(() =>
                            {
                                View.IsBusy = false;
                                if (onSuccessComplete != null)
                                    onSuccessComplete();
                            });
                    }
                });
        }

    }
}
