using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using outcold.MoMaSy.BindingModels;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Views;

namespace outcold.MoMaSy.Presenters
{
    internal class MessagesViewPresenter : Presenter<IMessagesView, MessagesBindingModel>
    {
        public MessagesViewPresenter(IMessagesView view)
            : base(view)
        {
            var notifyService = IoC.Resolve<INotifyService>();
            notifyService.ShowMessage += OnShowMessage;
        }

        private void OnShowMessage(object sender, MessageEventArgs message)
        {
            var messageBindinModel = new MessageBindingModel(message.Message, message.Type);
            messageBindinModel.PropertyChanged += MessagePropertyChanged;
            BindingModel.Messages.Add(messageBindinModel);

            // TODO: Simple temp way to hide messages
            Task.Factory.StartNew(() => { Thread.Sleep(5000); })
                .ContinueWith((t) => { BindingModel.Messages.Remove(messageBindinModel); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void MessagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == PropertyNameExtractor.GetPropertyName((x) => ))

            MessageBindingModel messageBindingModel = sender as MessageBindingModel;
            Debug.Assert(messageBindingModel != null, "messageBindingModel != null");



            messageBindingModel.PropertyChanged -= MessagePropertyChanged;
        }


    }
}
