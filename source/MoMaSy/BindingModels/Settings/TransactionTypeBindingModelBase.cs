using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal abstract class TransactionTypeBindingModelBase : HierarchicalItemBindingModel<TransactionTypeBindingModelBase, ITransactionType>
    {
        public TransactionTypeBindingModelBase(TransactionTypeBindingModelBase parent, ITransactionType item)
            : base(parent, item)
        {

        }

        public abstract string TransactionTypeName
        {
            get;
            set;
        }

        public abstract bool IsSystem
        {
            get;
        }
    }
}
