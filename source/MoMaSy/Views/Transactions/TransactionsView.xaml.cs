using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Presenters.Transactions;

namespace outcold.MoMaSy.Views.Transactions
{
    interface ITransactionsView : IView
    {

    }

    /// <summary>
    /// Interaction logic for TransactionsView.xaml
    /// </summary>
    public partial class TransactionsView : View, ITransactionsView
    {
        private TransactionsViewPresenter _presenter;

        public TransactionsView()
        {
            InitializeComponent();
            _presenter = CreatePresenter<TransactionsViewPresenter>();
        }

        private void AddTransactionClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _presenter.AddTransaction();
        }

        private void SaveTransactionClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _presenter.SaveTransaction();
        }

        private void CancelTransactionClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _presenter.CancelEditTransaction();
        }
    }
}
