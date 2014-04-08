using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.BindingModels.Transactions
{
    internal class TransactionItemBindingModel : CollectionItemBindingModel<TransactionItemBindingModel, ITransaction>
    {
        private DateTime _date;
        private decimal _incomeValue;
        private decimal _expenseValue;
        private ITransactionType _transactionType;
        private IAccount _accountFrom;
        private IAccount _accountTo;
        private string _comment;
        private TransactionKind _transactionKind;
        private bool _isHidden;

        private ITransactionType _systemTransferTransactionType;

        public TransactionItemBindingModel(ITransaction transaction)
            : base(transaction)
        {
            AddValidationFor(() => Account)
                .When(x => x.Account == null)
                .Show(() => TransactionKind == Model.TransactionKind.Transfer ?
                        StringResources.ErrMsg_Transaction_SetAccountFrom : StringResources.ErrMsg_Transaction_SetAccount);
            AddValidationFor(() => AccountTo)
                .When(x => x.TransactionKind == TransactionKind.Transfer && x.AccountTo == null)
                .Show(StringResources.ErrMsg_Transaction_SetAccountTo);

            _systemTransferTransactionType = IoC.Resolve<ITransactionTypesService>().GetTransferSystemTransactionType();
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value != _date)
                {
                    _date = value;
                    RaisePropertyChanged(() => Date);
                }
            }
        }

        public decimal Value
        {
            get
            {
                switch (TransactionKind)
                {
                    case Model.TransactionKind.Income:
                        return IncomeValue;
                    case Model.TransactionKind.Transfer:
                    case Model.TransactionKind.Expense:
                        return ExpenseValue;
                    default:
                        throw new NotSupportedException("Unsupported TransactionKind");
                }
            }
            set
            {
                switch (TransactionKind)
                {
                    case Model.TransactionKind.Income:
                        IncomeValue = value;
                        break;
                    case Model.TransactionKind.Transfer:
                    case Model.TransactionKind.Expense:
                        ExpenseValue = value;
                        break;
                    default:
                        throw new NotSupportedException("Unsupported TransactionKind");
                }
                RaisePropertyChanged(() => Value);
            }
        }

        public decimal IncomeValue
        {
            get
            {
                return _incomeValue;
            }
            set
            {
                if (value != _incomeValue)
                {
                    _incomeValue = value;
                    RaisePropertyChanged(() => IncomeValue);
                }
            }
        }

        public decimal ExpenseValue
        {
            get
            {
                return _expenseValue;
            }
            set
            {
                if (value != _expenseValue)
                {
                    _expenseValue = value;
                    RaisePropertyChanged(() => ExpenseValue);
                }
            }
        }

        public ITransactionType TransactionType
        {
            get
            {
                return _transactionType;
            }
            set
            {
                if (value != _transactionType)
                {
                    _transactionType = value;
                    RaisePropertyChanged(() => TransactionType);
                }
            }
        }

        public TransactionKind TransactionKind
        {
            get
            {
                return _transactionKind;
            }
            set
            {
                if (value != _transactionKind)
                {
                    var account = Account;
                    var transactionValue = Value;
                    _transactionKind = value;
                    RaisePropertyChanged(() => TransactionKind);
                    Account = account;
                    Value = transactionValue;
                }
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    RaisePropertyChanged(() => Comment);
                }
            }
        }

        public bool IsHidden
        {
            get
            {
                return _isHidden;
            }
            set
            {
                if (value != _isHidden)
                {
                    _isHidden = value;
                    RaisePropertyChanged(() => IsHidden);
                }
            }
        }

        public IAccount Account
        {
            get
            {
                switch (TransactionKind)
                {
                    case Model.TransactionKind.Income:
                        return AccountTo;
                    case Model.TransactionKind.Transfer:
                    case Model.TransactionKind.Expense:
                        return AccountFrom;
                    default:
                        throw new NotSupportedException("Unsupported TransactionKind");
                }
            }
            set
            {
                switch(TransactionKind)
                {
                    case Model.TransactionKind.Income:
                        AccountTo = value;
                        break;
                    case Model.TransactionKind.Transfer:
                    case Model.TransactionKind.Expense:
                        AccountFrom = value;
                        break;
                    default:
                        throw new NotSupportedException("Unsupported TransactionKind");
                }
                RaisePropertyChanged(() => Account);
            }
        }

        public IAccount AccountFrom
        {
            get
            {
                return _accountFrom;
            }
            set
            {
                if (value != _accountFrom)
                {
                    _accountFrom = value;
                    RaisePropertyChanged(() => AccountFrom);
                }
            }
        }

        public IAccount AccountTo
        {
            get
            {
                return _accountTo;
            }
            set
            {
                if (value != _accountTo)
                {
                    _accountTo = value;
                    RaisePropertyChanged(() => AccountTo);
                }
            }
        }

        public override bool HasChanges
        {
            get 
            {
                return _date != Item.Date
                    || _incomeValue != Item.IncomeValue
                    || _expenseValue != Item.ExpenseValue
                    || _transactionKind != Item.TransactionKind
                    || !object.Equals(_transactionType, Item.TransactionType)
                    || !string.Equals(_comment, Item.Comment)
                    || !object.Equals(_accountFrom, Item.AccountFrom)
                    || !object.Equals(_accountTo, Item.AccountTo);
            }
        }

        protected override void LoadData()
        {
            _date = Item.Date;
            _incomeValue = Item.IncomeValue;
            _expenseValue = Item.ExpenseValue;
            _transactionType = Item.TransactionType;
            _accountFrom = Item.AccountFrom;
            _accountTo = Item.AccountTo;
            _comment = Item.Comment;
            _transactionKind = Item.TransactionKind;
        }

        protected override void SaveData()
        {
            Item.Date = _date;
            
            Item.IncomeValue = _incomeValue;
            Item.ExpenseValue = _expenseValue;
            Item.TransactionType = _transactionType;
            Item.AccountFrom = _accountFrom;
            Item.AccountTo = _accountTo;
            Item.Comment = _comment;
            Item.TransactionKind = _transactionKind;
            switch (_transactionKind)
            {
                case TransactionKind.Income:
                    Item.ExpenseValue = Item.ExpenseValue = 0;
                    Item.AccountFrom = Item.AccountFrom = null;
                    break;
                case TransactionKind.Expense:
                    Item.IncomeValue = Item.IncomeValue = 0;
                    Item.AccountTo = Item.AccountTo = null;
                    break;
                case TransactionKind.Transfer:
                    Item.TransactionType = _systemTransferTransactionType;
                    break;
            }
        }
    }
}
