using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal class SystemTransactionTypeBindingModel : TransactionTypeBindingModelBase
    {
        public SystemTransactionTypeBindingModel()
            : base(null, new TransactionTypeModel(IoC.Instance, new TransactionType()))
        {

        }

        public override string TransactionTypeName
        {
            get
            {
                return StringResources.TransactionType_CollectionName;
            }
            set
            {
                throw new NotSupportedException("Cannot set name for system transaction type");
            }
        }

        public override bool IsSystem
        {
            get { return true; }
        }

        public override bool HasChanges
        {
            get { return false; }
        }

        protected override void LoadData()
        {
            
        }

        protected override void SaveData()         
        {
            Debug.Assert(false, "Unsupported: Saving system transaction type.");
        }
    }
}
