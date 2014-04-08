using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.Model
{
    internal interface ITransaction 
    {
        DateTime Date { get; set; }
        decimal IncomeValue { get; set; }
        decimal ExpenseValue { get; set; }

        ITransactionType TransactionType { get; set; }

        IAccount AccountFrom { get; set; }
        IAccount AccountTo { get; set; }

        string Comment { get; set; }

        bool IsHidden { get; set; }

        TransactionKind TransactionKind { get; set; }
    }

    internal class TransactionModel : EntityModel<Transaction>, ITransaction
    {
        private ITransactionTypesService _transactionTypesService;
        private IAccountsService _accountsService;

        public TransactionModel(IoC ioc, Transaction transaction)
            : base(transaction)
        {
            _transactionTypesService = ioc.Resolve<ITransactionTypesService>();
            _accountsService = ioc.Resolve<IAccountsService>();
        }

        public DateTime Date
        {
            get { return DataItem.Date; }
            set { DataItem.Date = value; }
        }

        public decimal IncomeValue
        {
            get { return DataItem.IncomeValue; }
            set { DataItem.IncomeValue = value; }
        }

        public decimal ExpenseValue
        {
            get { return DataItem.ExpenseValue; }
            set { DataItem.ExpenseValue = value; }
        }

        public bool IsHidden
        {
            get { return DataItem.IsHidden; }
            set { DataItem.IsHidden = value; }
        }

        public ITransactionType TransactionType
        {
            get
            {
                return _transactionTypesService.Get(DataItem.TransactionTypeId);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Transaction Type cannot be null");

                if (value != null && !(value is TransactionTypeModel))
                    throw new ArgumentException("value", "Couldn't get information about Entity. Should be AccountModel.");

                DataItem.TransactionTypeId = ((TransactionTypeModel)value).Id;
            }
        }

        public IAccount AccountFrom
        {
            get
            {
                if (!DataItem.AccountFromId.HasValue)
                    return null;

                return _accountsService.Get(DataItem.AccountFromId.Value);
            }
            set
            {
                if (value != null && !(value is AccountModel))
                    throw new ArgumentException("value", "Couldn't get information about Entity. Should be AccountModel.");

                DataItem.AccountFromId = value == null ? null : (int?)((AccountModel)value).Id;
            }
        }

        public IAccount AccountTo
        {
            get
            {
                if (!DataItem.AccountToId.HasValue)
                    return null;

                return _accountsService.Get(DataItem.AccountToId.Value);
            }
            set
            {
                if (value != null && !(value is AccountModel))
                    throw new ArgumentException("value", "Couldn't get information about Entity. Should be AccountModel.");

                DataItem.AccountToId = value == null ? null : (int?)((AccountModel)value).Id;
            }
        }
        

        public string Comment
        {
            get { return DataItem.Comment; }
            set { DataItem.Comment = value; }
        }

        public TransactionKind TransactionKind
        {
            get { return (TransactionKind)DataItem.TransactionKindId; }
            set { DataItem.TransactionKindId = (byte)value; }
        }
    }
}
