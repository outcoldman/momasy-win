using System;
using outcold.MoMaSy.Core.Presentation;

namespace outcold.MoMaSy.Core
{
    internal enum MessageType
    {
        Information = 0,
        Error = 1
    }

    internal class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string message, MessageType messageType)
        {
            Message = message;
            Type = messageType;
        }

        public MessageType Type { get; private set; }
        public string Message { get; private set; }
    }

    internal interface INotifyService
    {
        event EventHandler<MessageEventArgs> ShowMessage;

        void ShowInformation(string message);
        void ShowError(string message);
    }

    internal class NotifyService : INotifyService
    {
        public event EventHandler<MessageEventArgs> ShowMessage;

        public void ShowInformation(string message)
        {
            RaiseShowMessage(message, MessageType.Information);
        }

        public void ShowError(string message)
        {
            RaiseShowMessage(message, MessageType.Error);
        }

        private void RaiseShowMessage(string message, MessageType messageType)
        {
            var handler = ShowMessage;
            if (handler != null)
            {
                App.Current.Dispatcher.Do(() => handler(this, new MessageEventArgs(message, messageType)));
            }
        }
    }
}
