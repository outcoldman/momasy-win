using System;
using System.Linq;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;

namespace outcold.MoMaSy.Services
{
    internal interface ITransactionTypesService : IDictionaryEntityService<ITransactionType>
    {
        ITransactionType CreateNew(ITransactionType parent);
        ITransactionType GetTransferSystemTransactionType();
        bool CanBeDeleted(ITransactionType transactionType, out string errorMessage);
    }

    internal class TransactionTypesService : DictionaryServiceBase<ITransactionType, TransactionType>, ITransactionTypesService
    {
        private ITransactionType _systemTransferTransactionType = null;

        public TransactionTypesService(IoC ioc)
            : base(ioc)
        {
            SetWhereCondition(x => !x.IsSystem);
        }

        protected override ITransactionType CreateContainer(TransactionType entity)
        {
            return new TransactionTypeModel(IoC, entity);
        }

        ITransactionType ITransactionTypesService.CreateNew(ITransactionType parent)
        {
            int? parentId = parent == null ? null : (int?)GetDataItem(parent).Id;

            var transactionType = new TransactionType()
            {
                TransactionTypeName = StringResources.TransactionType_NewName,
                TransactionParentTypeId = parentId
            };

            var result = CreateContainer(transactionType);
            ((ITransactionTypesService)this).Save(result);
            return result;
        }

        bool ITransactionTypesService.CanBeDeleted(ITransactionType transactionType, out string errorMessage)
        {
            var dataItem = GetDataItem(transactionType);

            if (dataItem.IsNew)
                throw new ArgumentException("Only saved transaction types can be verified", "transactionType");

            errorMessage = null;

            using (var context = GetContext())
            {
                if (context.Transactions.Any(x => x.TransactionTypeId == dataItem.Id))
                    errorMessage = StringResources.ErrMsg_TransactionType_UsedByTransaction;
            }

            return errorMessage == null;
        }


        public ITransactionType GetTransferSystemTransactionType()
        {
            if (_systemTransferTransactionType == null)
            {
                using (var context = GetContext())
                {
                    _systemTransferTransactionType = CreateContainer(context.TransactionTypes.First(x => x.IsSystem));
                }
            }
            return _systemTransferTransactionType;
        }

        public override ITransactionType Get(int id)
        {
            var systemTransactionType = GetTransferSystemTransactionType();
            if (id == ((TransactionTypeModel)systemTransactionType).Id)
                return systemTransactionType;

            return base.Get(id);
        }
    }
}
