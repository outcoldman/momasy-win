using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.Model
{
    internal interface ITransactionType 
    {
        string TransactionTypeName { get; set; }
        bool IsHidden { get; set; }

        ITransactionType Parent { get; set; }
        IEnumerable<ITransactionType> VisibleChildren { get; }
    }

    internal class TransactionTypeModel : EntityModel<TransactionType>, ITransactionType
    {
        private ITransactionTypesService _transactionTypesService;

        public TransactionTypeModel(IoC ioc, TransactionType transactionType)
            : base(transactionType)
        {
            _transactionTypesService = ioc.Resolve<ITransactionTypesService>();
        }

        public string TransactionTypeName
        {
            get { return DataItem.TransactionTypeName; }
            set { DataItem.TransactionTypeName = value; }
        }

        public bool IsHidden
        {
            get { return DataItem.IsHidden; }
            set { DataItem.IsHidden = value; }
        }

        public ITransactionType Parent
        {
            get
            {
                if (!DataItem.TransactionParentTypeId.HasValue)
                    return null;

                return _transactionTypesService.Get(DataItem.TransactionParentTypeId.Value);
            }
            set
            {
                if (value != null && !(value is TransactionTypeModel))
                    throw new ArgumentException("value", "Couldn't get information about Entity. Should be AccountModel.");

                DataItem.TransactionParentTypeId = value == null ? null : (int?)((TransactionTypeModel)value).Id;
            }
        }


        public IEnumerable<ITransactionType> VisibleChildren
        {
            get
            {
                return _transactionTypesService.GetAll().Where(x => 
                {
                    if (x.IsHidden)
                        return false;

                    if (!(x is TransactionTypeModel))
                        throw new ArgumentException("value", "Couldn't get information about Entity. Should be AccountModel.");

                    TransactionType transactionType = ((TransactionTypeModel)x).DataItem;
                    return transactionType.TransactionParentTypeId.HasValue && transactionType.TransactionParentTypeId.Value == Id; 
                }).OrderBy(x => x.TransactionTypeName);
            }
        }
    }
}
