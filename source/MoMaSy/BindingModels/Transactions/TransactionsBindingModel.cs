using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core.Presentation;

namespace outcold.MoMaSy.BindingModels.Transactions
{
    internal class TransactionsBindingModel : BindingModel
    {
        private TransactionItemBindingModel _transactionBindindModel;

        public TransactionsBindingModel()
        {
            ListBindingModel = new TransactionsListBindingModel();
            FilterBindingModel = new TransactionsFilterBindingModel();
        }

        public TransactionsListBindingModel ListBindingModel { get; private set; }

        public TransactionsFilterBindingModel FilterBindingModel { get; private set; }

        public TransactionItemBindingModel TransactionBindingModel 
        {
            get { return _transactionBindindModel; }
            set
            {
                if (value != _transactionBindindModel)
                {
                    _transactionBindindModel = value;
                    RaisePropertyChanged(() => IsTransactionEditVisible);
                    RaisePropertyChanged(() => TransactionBindingModel);
                }
            }
        }

        public bool IsTransactionEditVisible
        {
            get { return TransactionBindingModel != null; }
        }
    }
}
