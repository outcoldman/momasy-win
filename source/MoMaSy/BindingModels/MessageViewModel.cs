using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;

namespace outcold.MoMaSy.BindingModels
{
    internal class MessageBindingModel : BindingModel
    {
        public MessageBindingModel(string message, MessageType messageType)
        {
            Message = message;
            Type = messageType;
        }

        public string Message
        {
            get;
            private set;
        }

        public MessageType Type
        {
            get;
            private set;
        }
    }
}
