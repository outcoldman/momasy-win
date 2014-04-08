using System.Collections.ObjectModel;
using outcold.MoMaSy.Core.Presentation;

namespace outcold.MoMaSy.BindingModels
{
    internal class MessagesBindingModel : BindingModel
    {
        public MessagesBindingModel()
        {
            Messages = new ObservableCollection<MessageBindingModel>();
        }

        public ObservableCollection<MessageBindingModel> Messages { get; private set; }
    }
}
