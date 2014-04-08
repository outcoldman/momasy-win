using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.BindingModels.Transactions
{
    internal class TransactionsListBindingModel : BindingModel
    {
        private ObservableCollection<TransactionItemBindingModel> _collection = new ObservableCollection<TransactionItemBindingModel>();
        private TransactionItemBindingModel _selectedTransaction;

        public ObservableCollection<TransactionItemBindingModel> Collection
        {
            get { return _collection; }
        }

        public TransactionItemBindingModel SelectedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                if (value != _selectedTransaction)
                {
                    _selectedTransaction = value;
                    RaisePropertyChanged(() => SelectedTransaction);
                }
            }
        }

        public void SetTransactions(IEnumerable<ITransaction> transactions)
        {
            _collection.Clear();
            foreach (var transaction in transactions)
            {
                _collection.Add(new TransactionItemBindingModel(transaction));
            }
        }
    }
}
