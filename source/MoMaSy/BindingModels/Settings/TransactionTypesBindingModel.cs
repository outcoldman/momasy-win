using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;

namespace outcold.MoMaSy.BindingModels.Settings
{
    internal class TransactionTypesBindingModel : BindingModel
    {
        private TransactionTypeBindingModelBase _root;

        public IEnumerable<TransactionTypeBindingModelBase> Roots
        {
            get
            {
                if (_root == null)
                    return null;
                else
                    return new Collection<TransactionTypeBindingModelBase> { _root };
            }
        }

        public TransactionTypeBindingModelBase SelectedTransactionTypeBindingModel
        {
            get
            {
                return FindSelected(_root) ?? _root;
            }
        }

        private TransactionTypeBindingModelBase FindSelected(TransactionTypeBindingModelBase parent)
        {
            TransactionTypeBindingModelBase result = null;

            foreach (var item in parent.Children)
            {
                if (item.IsSelected)
                {
                    result = item;
                }
                else
                {
                    result = FindSelected(item);
                }

                if (result != null)
                    break;
            }

            return result;
        }


        internal void SetTransactionTypes(IEnumerable<ITransactionType> transactionTypes)
        {
            _root = new SystemTransactionTypeBindingModel();
            _root.SetChildren(GetChildren(_root, transactionTypes, transactionTypes.Where(x => x.Parent == null)));
            RaisePropertyChanged(() => Roots);
        }

        private IEnumerable<TransactionTypeBindingModelBase> GetChildren(TransactionTypeBindingModelBase parent, IEnumerable<ITransactionType> allTypes, IEnumerable<ITransactionType> topLevel)
        {
            foreach (var type in topLevel)
            {
                TransactionTypeBindingModel bindingModel = new TransactionTypeBindingModel(parent, type);
                var childLevel = allTypes.Where(x => type.Equals(x.Parent));
                bindingModel.SetChildren(GetChildren(bindingModel, allTypes, childLevel));
                yield return bindingModel;
            }
        }
    }
}
