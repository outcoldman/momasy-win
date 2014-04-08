using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Presenters;

namespace outcold.MoMaSy.Views
{
    internal interface IMessagesView : IView
    {
    }

    /// <summary>
    /// Interaction logic for MessagesView.xaml
    /// </summary>
    public partial class MessagesView : View, IMessagesView
    {
        public MessagesView()
        {
            InitializeComponent();
            CreatePresenter<MessagesViewPresenter>();
        }
    }
}
