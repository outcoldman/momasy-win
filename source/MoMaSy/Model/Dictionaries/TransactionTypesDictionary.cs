using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Services;

namespace outcold.MoMaSy.Model.Dictionaries
{
    internal class TransactionTypesDictionary : DomainDictionaryBase<ITransactionType, ITransactionTypesService>
    {
        protected override IEnumerable<ITransactionType> PrepareCollection(IEnumerable<ITransactionType> collection)
        {
            return collection.Where(x => !x.IsHidden /*&& x.Parent == null*/).OrderBy(x => x.TransactionTypeName);
        }

        public ITransactionType SystemTransactionType
        {
            get
            {
                var service = IoC.Instance.Resolve<ITransactionTypesService>();
                return service.GetTransferSystemTransactionType();
            }
        }
    }
}
