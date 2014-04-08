using System;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal class TransactionTypeBindingModel : TransactionTypeBindingModelBase
    {
        private string _transactionTypeName;
        private bool _isHidden;

        public TransactionTypeBindingModel(TransactionTypeBindingModelBase parent, ITransactionType transactionType)
            : base(parent, transactionType)
        {
            AddValidationFor(() => TransactionTypeName)
                .When(x => string.IsNullOrWhiteSpace(x.TransactionTypeName))
                .Show(StringResources.ValidationMsg_TransactionType_Name);
        }

        public override string TransactionTypeName
        {
            get
            {
                return _transactionTypeName;
            }
            set
            {
                if (!string.Equals(_transactionTypeName, value, StringComparison.CurrentCulture))
                {
                    _transactionTypeName = value;
                    RaisePropertyChanged(() => TransactionTypeName);
                    RaisePropertyChanged(() => HasChanges);
                }
            }
        }

        public bool IsHidden
        {
            get { return _isHidden || (Parent != null && Parent is TransactionTypeBindingModel && ((TransactionTypeBindingModel)Parent).IsHidden); }
            set
            {
                if (_isHidden != value)
                {
                    _isHidden = value;
                    RaisePropertyChanged(() => HasChanges);
                    RaiseIsHiddenChanged(this);
                }
            }
        }

        public override bool IsSystem
        {
            get { return false; }
        }

        public override bool HasChanges
        {
            get
            {
                return Item == null || Item.IsHidden != _isHidden ||
                    !string.Equals(Item.TransactionTypeName, _transactionTypeName, StringComparison.CurrentCulture);
            }
        }

        protected override void LoadData()
        {
            TransactionTypeName = Item.TransactionTypeName;
            IsHidden = Item.IsHidden;
        }

        protected override void SaveData()
        {
            Item.TransactionTypeName = _transactionTypeName;
            Item.IsHidden = _isHidden;
        }

        private void RaiseIsHiddenChanged(TransactionTypeBindingModelBase bm)
        {
            ((TransactionTypeBindingModel)bm).RaisePropertyChanged(() => IsHidden);
            if (bm.Children != null)
            {
                foreach (var child in bm.Children)
                {
                    RaiseIsHiddenChanged(child);
                }
            }
        }
    }
}
